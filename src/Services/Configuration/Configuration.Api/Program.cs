//2023 (c) TD Synnex - All Rights Reserved.



using Identity.Security.SecureVault;

var appName = "Configuration.API";
var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    Args = args,
    ApplicationName = typeof(Program).Assembly.FullName,
    ContentRootPath = Directory.GetCurrentDirectory()
});

builder.Configuration.SetBasePath(Directory.GetCurrentDirectory());
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
builder.Configuration.AddEnvironmentVariables();
builder.WebHost.ConfigureKestrel(options =>
{
    var ports = GetDefinedPorts(builder.Configuration);
    options.Listen(IPAddress.Any, ports.httpPort, listenOptions =>
    {
        listenOptions.Protocols = HttpProtocols.Http1AndHttp2;
    });

    options.Listen(IPAddress.Any, ports.grpcPort, listenOptions =>
    {
        listenOptions.Protocols = HttpProtocols.Http2;
    });

});
builder.WebHost.CaptureStartupErrors(false);
builder.Host.UseSerilog(CreateSerilogLogger(builder.Configuration));

builder.Services
.AddSecureVault(builder.Configuration, builder)
.AddApplicationInsights(builder.Configuration)
.AddCustomMvc()
.AddHealthChecks(builder.Configuration)
.AddCustomDbContext(builder.Configuration);

builder.Services.AddDistributedMemoryCache();
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseDeveloperExceptionPage();
    ///app.UseExceptionHandler("/Home/Error");
}
var pathBase = app.Configuration["PATH_BASE"];
if (!string.IsNullOrEmpty(pathBase))
{
    app.UsePathBase(pathBase);
}

app.UseRouting();
app.UseCors("CorsPolicy");


//app.MapGrpcService<OrderingService>();
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
//ConfigureEventBus(app);
try
{
    Log.Information("Applying migrations ({ApplicationContext})...", Program.AppName);
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<ConfigurationContext>();
    var env = app.Services.GetService<IWebHostEnvironment>();


    var envName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
    Log.Information($"EnvironmentVariable: ASPNETCORE_ENVIRONMENT {envName} EnvironmentName:{env?.EnvironmentName}");
    builder.Configuration.AddJsonFile($"appsettings.{envName}.json", optional: true, reloadOnChange: true);

    // var logger = app.Services.GetService<ILogger<OrderingContextSeed>>();
    //await context.Database.MigrateAsync();

    //  await new OrderingContextSeed().SeedAsync(context, env, settings, logger);
    //var integEventContext = scope.ServiceProvider.GetRequiredService<IntegrationEventLogContext>();
    //await integEventContext.Database.MigrateAsync();

    Log.Information("Starting web host ({ApplicationContext})...", Program.AppName);
    await app.RunAsync();

    return 0;
}
catch (Exception ex)
{
    Log.Fatal(ex, "Program terminated unexpectedly ({ApplicationContext})!", Program.AppName);
    return 1;
}
finally
{
    Log.CloseAndFlush();
}
void ConfigureEventBus(IApplicationBuilder app)
{
    var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();

    //eventBus.Subscribe<OrderReceivedIntegrationEvent, IIntegrationEventHandler<OrderReceivedIntegrationEvent>>();
    //eventBus.Subscribe<OrderErpResponseReceivedIntegrationEvent, IIntegrationEventHandler<OrderErpResponseReceivedIntegrationEvent>>();
}
Serilog.ILogger CreateSerilogLogger(IConfiguration configuration)
{
    var seqServerUrl = configuration["Serilog:SeqServerUrl"];
    var logstashUrl = configuration["Serilog:LogstashgUrl"];
    return new LoggerConfiguration()
        .MinimumLevel.Verbose()
        .Enrich.WithProperty("ApplicationContext", Program.AppName)
        .Enrich.FromLogContext()
        .WriteTo.Console()
        .WriteTo.Seq(string.IsNullOrWhiteSpace(seqServerUrl) ? "http://seq" : seqServerUrl)
        .WriteTo.Http(string.IsNullOrWhiteSpace(logstashUrl) ? "http://logstash:8080" : logstashUrl, null)
        .ReadFrom.Configuration(configuration)
        .CreateLogger();
}
(int httpPort, int grpcPort) GetDefinedPorts(IConfiguration config)
{
    var grpcPort = config.GetValue("GRPC_PORT", 5001);
    var port = config.GetValue("PORT", 80);
    return (port, grpcPort);
}
public partial class Program
{

    public static string Namespace = typeof(Program).Assembly.GetName().Name;
    public static string AppName = Namespace.Substring(Namespace.LastIndexOf('.', Namespace.LastIndexOf('.') - 1) + 1);
}
static class CustomExtensionsMethods
{
    public static IServiceCollection AddApplicationInsights(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddApplicationInsightsTelemetry(configuration);
        services.AddApplicationInsightsKubernetesEnricher();

        return services;
    }

    public static IServiceCollection AddCustomMvc(this IServiceCollection services)
    {
        // Add framework services.
        services.AddControllers(options =>
        {
            options.Filters.Add(typeof(HttpGlobalExceptionFilter));
        })
            // Added for functional tests
            //  .AddApplicationPart(typeof(OrderController).Assembly)
            .AddJsonOptions(options => options.JsonSerializerOptions.WriteIndented = true)
            .AddXmlSerializerFormatters();

        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy",
                builder => builder
                .SetIsOriginAllowed((host) => true)
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());
        });

        return services;
    }

    public static IServiceCollection AddHealthChecks(this IServiceCollection services, IConfiguration configuration)
    {
        var hcBuilder = services.AddHealthChecks();

        hcBuilder.AddCheck("self", () => HealthCheckResult.Healthy());

        //hcBuilder
        //    .AddSqlServer(
        //        configuration["ConnectionString"],
        //        name: "preprovisiong-check",
        //        tags: new string[] { "preprovisioningcheckdb" });

        //if (configuration.GetValue<bool>("AzureServiceBusEnabled"))
        //{
        //    hcBuilder
        //        .AddAzureServiceBusTopic(
        //            configuration["EventBusConnection"],
        //            topicName: "s1_int_event_bus",
        //            name: "preprovisioning-servicebus-check",
        //            tags: new string[] { "servicebus" });
        //}
        //else
        //{
        //    hcBuilder
        //        .AddRabbitMQ(
        //            $"amqp://{configuration["EventBusConnection"]}",
        //            name: "ordering-rabbitmqbus-check",
        //            tags: new string[] { "rabbitmqbus" });
        //}

        return services;
    }

    public static IServiceCollection AddCustomDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ConfigurationContext>(options =>
        {
            options.UseSqlServer(configuration["ConnectionString"],
                sqlServerOptionsAction: sqlOptions =>
                {
                    sqlOptions.MigrationsAssembly(typeof(Program).GetTypeInfo().Assembly.GetName().Name);
                    sqlOptions.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                });
        },
                    ServiceLifetime.Scoped  //Showing explicitly that the DbContext is shared across the HTTP request scope (graph of objects started in the HTTP request)
                );

        services.AddDbContext<IntegrationEventLogContext>(options =>
        {
            options.UseSqlServer(configuration["ConnectionString"],
                                    sqlServerOptionsAction: sqlOptions =>
                                    {
                                        sqlOptions.MigrationsAssembly(typeof(Program).GetTypeInfo().Assembly.GetName().Name);
                                        //Configuring Connection Resiliency: https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency 
                                        sqlOptions.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                                    });
        });

        return services;
    }

    public static IServiceCollection AddCustomSwagger(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "eShopOnContainers - Ordering HTTP API",
                Version = "v1",
                Description = "The Ordering Service HTTP API"
            });
            options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.OAuth2,
                Flows = new OpenApiOAuthFlows()
                {
                    Implicit = new OpenApiOAuthFlow()
                    {
                        AuthorizationUrl = new Uri($"{configuration.GetValue<string>("ConfigurationUrlExternal")}/connect/authorize"),
                        TokenUrl = new Uri($"{configuration.GetValue<string>("ConfigurationUrlExternal")}/connect/token"),
                        Scopes = new Dictionary<string, string>()
                        {
                            { "orders", "Ordering API" }
                        }
                    }
                }
            });
            options.OperationFilter<AuthorizeCheckOperationFilter>();
        });
        return services;
    }

    public static IServiceCollection AddCustomIntegrations(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpContextAccessor();
        services.AddHttpClient("client")
             .SetHandlerLifetime(TimeSpan.FromMinutes(5));

        services.AddHttpClient("ERPClient", httpClient =>
        { });

        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        //services.AddTransient<IConfigurationService, ConfigurationService>();
        services.AddTransient<Func<DbConnection, IIntegrationEventLogService>>(
            sp => (DbConnection c) => new IntegrationEventLogService(c));

        if (configuration.GetValue<bool>("AzureServiceBusEnabled"))
        {
            services.AddSingleton<IServiceBusPersisterConnection>(sp =>
            {
                var serviceBusConnectionString = configuration["EventBusConnection"];

                var subscriptionClientName = configuration["SubscriptionClientName"];

                return new DefaultServiceBusPersisterConnection(serviceBusConnectionString);
            });
        }
        else
        {
            //services.AddSingleton<IRabbitMQPersistentConnection>(sp =>
            //{
            //    var logger = sp.GetRequiredService<ILogger<DefaultRabbitMQPersistentConnection>>();


            //    var factory = new ConnectionFactory()
            //    {
            //        HostName = configuration["EventBusConnection"],
            //        DispatchConsumersAsync = true
            //    };

            //    if (!string.IsNullOrEmpty(configuration["EventBusUserName"]))
            //    {
            //        factory.UserName = configuration["EventBusUserName"];
            //    }

            //    if (!string.IsNullOrEmpty(configuration["EventBusPassword"]))
            //    {
            //        factory.Password = configuration["EventBusPassword"];
            //    }

            //    var retryCount = 5;
            //    if (!string.IsNullOrEmpty(configuration["EventBusRetryCount"]))
            //    {
            //        retryCount = int.Parse(configuration["EventBusRetryCount"]);
            //    }

            //    return new DefaultRabbitMQPersistentConnection(factory, logger, retryCount);
            //});
        }

        return services;
    }

    public static IServiceCollection AddCustomConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions();
        //services.Configure<PreProvisioningCheckSettings>(configuration);
        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.InvalidModelStateResponseFactory = context =>
            {
                var problemDetails = new ValidationProblemDetails(context.ModelState)
                {
                    Instance = context.HttpContext.Request.Path,
                    Status = StatusCodes.Status400BadRequest,
                    Detail = "Please refer to the errors property for additional details."
                };

                return new BadRequestObjectResult(problemDetails)
                {
                    ContentTypes = { "application/problem+json", "application/problem+xml" }
                };
            };
        });

        return services;
    }


    public static IServiceCollection AddSecureVault(this IServiceCollection services, IConfiguration configuration, WebApplicationBuilder builder)
    {
        var secureVaultService = new AzureKeyVaultHelper(configuration);
        services.AddSingleton<ISecureVaultHelper>(secureVaultService);
        return services;
    }

    public static IServiceCollection AddEventBus(this IServiceCollection services, IConfiguration configuration)
    {
        if (configuration.GetValue<bool>("AzureServiceBusEnabled"))
        {
            services.AddSingleton<IEventBus, EventBusServiceBus>(sp =>
            {
                var serviceBusPersisterConnection = sp.GetRequiredService<IServiceBusPersisterConnection>();
                var logger = sp.GetRequiredService<ILogger<EventBusServiceBus>>();
                var eventBusSubcriptionsManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();
                string subscriptionName = configuration["SubscriptionClientName"];
                string topicName = configuration["EventBusTopic"];

                return new EventBusServiceBus(serviceBusPersisterConnection, logger,
                    eventBusSubcriptionsManager, sp, subscriptionName, topicName);
            });
        }
        else
        {
            //services.AddSingleton<IEventBus, EventBusRabbitMQ>(sp =>
            //{
            //    var subscriptionClientName = configuration["SubscriptionClientName"];
            //    var rabbitMQPersistentConnection = sp.GetRequiredService<IRabbitMQPersistentConnection>();
            //    var logger = sp.GetRequiredService<ILogger<EventBusRabbitMQ>>();
            //    var eventBusSubcriptionsManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();

            //    var retryCount = 5;
            //    if (!string.IsNullOrEmpty(configuration["EventBusRetryCount"]))
            //    {
            //        retryCount = int.Parse(configuration["EventBusRetryCount"]);
            //    }

            //    return new EventBusRabbitMQ(rabbitMQPersistentConnection, logger, sp, eventBusSubcriptionsManager, subscriptionClientName, retryCount);
            //});
        }

        services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();

        return services;
    }

}
