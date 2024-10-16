using Azure.Identity;
using Common.Configuration;
using VendorConfiguration.Api.Infrastructure.AutofacModules;
using VendorConfiguration.Api.Infrastructure.Extensions;
using CustomExtensionsMethods = VendorConfiguration.Api.Configuration.CustomExtensionsMethods;

namespace VendorConfiguration.Api
{
    /// <summary>
    /// Entry point for the VendorConfiguration API.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// Gets the namespace of the current assembly.
        /// </summary>
        public static string Namespace { get; private set; } = typeof(Program).Assembly.GetName().Name ?? string.Empty;

        /// <summary>
        /// Gets the application name extracted from the namespace.
        /// </summary>
        public static string AppName { get; private set; } = Namespace[(Namespace.LastIndexOf('.', Namespace.LastIndexOf('.') - 1) + 1)..];

        /// <summary>
        /// Main method that sets up the Web Application Builder and runs the application.
        /// </summary>
        /// <param name="args">The arguments provided during application startup.</param>
        /// <returns>An integer representing the application exit code.</returns>
        private static async Task<int> Main(string[] args)
        {
            ArgumentGuard.NotNull(args, nameof(args));

            var envName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            var builder = WebApplication.CreateBuilder(new WebApplicationOptions
            {
                Args = args,
                ApplicationName = typeof(Program).Assembly.FullName,
                ContentRootPath = Directory.GetCurrentDirectory()
            });

            builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

            if (builder.Configuration.GetValue("UseVault", false))
            {
                builder.Configuration.AddAzureKeyVault(new Uri($"https://{builder.Configuration["Vault:Name"]}.vault.azure.net/"), new DefaultAzureCredential(),
                     new AzureKeyVaultConfigurationOptions
                     {
                         ReloadInterval = TimeSpan.FromMinutes(60)
                     }
                    );
            }
            builder.Configuration.SetBasePath(Directory.GetCurrentDirectory());

            builder.WebHost.ConfigureKestrel(options =>
            {
                var (httpPort, grpcPort) = GetDefinedPorts(builder.Configuration);

                options.Listen(IPAddress.Any, httpPort, listenOptions =>
                {
                    listenOptions.Protocols = HttpProtocols.Http1AndHttp2;
                });
            });
            builder.WebHost.CaptureStartupErrors(false);
            CreateSerilogLogger(builder.Configuration);
            builder.Host.UseSerilog(CreateSerilogLogger(builder.Configuration));

            // Add custom application services and configurations
            CustomExtensionsMethods.AddEventBus(CustomExtensionsMethods.AddCustomSwagger(CustomExtensionsMethods.AddCustomIntegrations(CustomExtensionsMethods.AddCustomDbContext(CustomExtensionsMethods.AddHealthChecks(builder.Services
                        .AddConfigurationMvc(builder.Configuration), builder.Configuration), builder.Configuration), builder.Configuration)
                    .AddApiBehaviorConfiguration(builder.Configuration)
                    .AddApplicationInsights(builder.Configuration), builder.Configuration), builder.Configuration);

            builder.Host.ConfigureContainer<ContainerBuilder>(conbuilder => conbuilder.RegisterModule(new MediatorModule()));
            builder.Host.ConfigureContainer<ContainerBuilder>(conbuilder => conbuilder.RegisterModule(new RepositoryModule()));

            var app = builder.Build();

            // Configure middleware pipeline
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseDeveloperExceptionPage();
            }

            var pathBase = app.Configuration["PATH_BASE"];
            switch (string.IsNullOrEmpty(pathBase))
            {
                case false:
                    app.UsePathBase(pathBase);
                    break;
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"{(!string.IsNullOrEmpty(pathBase) ? pathBase : string.Empty)}/swagger/v1/swagger.json", "VendorConfiguration.Api V1");
                c.OAuthClientId("Vendorconfigurationswaggerui");
                c.OAuthAppName("Vendor Configuration Swagger UI");
            });

            app.UseRouting();
            app.UseCors("CorsPolicy");

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapDefaultControllerRoute();
            app.MapControllers();

            app.MapHealthChecks("/hc", new HealthCheckOptions()
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });

            app.MapHealthChecks("/liveness", new HealthCheckOptions
            {
                Predicate = r => r.Name.Contains("self")
            });

            try
            {
                var env = app.Services.GetService<IWebHostEnvironment>();
                Log.Information("EnvironmentVariable: ASPNETCORE_ENVIRONMENT {EnvName} EnvironmentName:{EnvironmentName}", envName, env?.EnvironmentName);
                Log.Information("Starting web host ({ApplicationContext})...", AppName);
                await app.RunAsync().ConfigureAwait(false);

                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Program terminated unexpectedly ({ApplicationContext})!", AppName);
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        /// <summary>
        /// Creates a Serilog logger instance.
        /// </summary>
        /// <param name="configuration">The application configuration settings.</param>
        /// <returns>A configured <see cref="Serilog.ILogger"/> instance.</returns>
        private static Serilog.ILogger CreateSerilogLogger(IConfiguration configuration)
        {
            var seqServerUrl = configuration["Serilog:SeqServerUrl"];
            var logstashUrl = configuration["Serilog:LogstashUrl"];
            return new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .ReadFrom.Configuration(configuration)
                .Enrich.WithProperty("ApplicationContext", AppName)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.Seq(string.IsNullOrWhiteSpace(seqServerUrl) ? "http://seq" : seqServerUrl)
                .WriteTo.Http(string.IsNullOrWhiteSpace(logstashUrl) ? "http://logstash:8080" : logstashUrl, null)
                .CreateLogger();
        }

        /// <summary>
        /// Retrieves the HTTP and gRPC port configurations from the application settings.
        /// </summary>
        /// <param name="config">The application configuration settings.</param>
        /// <returns>A tuple containing the HTTP and gRPC ports.</returns>
        private static (int httpPort, int grpcPort) GetDefinedPorts(IConfiguration config)
        {
            var grpcPort = config.GetValue("GRPC_PORT", 5001);
            var port = config.GetValue("PORT", 80);
            return (port, grpcPort);
        }
    }
}
