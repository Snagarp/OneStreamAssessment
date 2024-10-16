#pragma warning disable CA2211, CA1303, CA8601, CA8602
using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Identity;
using Common.Configuration;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Serilog;
using Web.VendorConfiguration.HttpAggregator.Services;

namespace Web.VendorConfiguration.HttpAggregator
{
    public static class Program
    {
        // Namespace and application name based on the assembly's name.
        public static string Namespace = typeof(Program)?.Assembly?.GetName()?.Name!;
        public static string AppName = Namespace[(Namespace.LastIndexOf('.', Namespace.LastIndexOf('.') - 1) + 1)..];

        // Main method that runs the application.
        private static async Task<int> Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Get the current environment (e.g., Development, Production)
            var envName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            // Set up Azure Key Vault if configured to use it
            if (builder.Configuration.GetValue("UseVault", false))
            {
                try
                {
                    // Add Azure Key Vault to the configuration pipeline
                    builder.Configuration.AddAzureKeyVault(
                        new Uri($"https://{builder.Configuration["Vault:Name"]}.vault.azure.net/"),
                        new DefaultAzureCredential(),
                        new AzureKeyVaultConfigurationOptions
                        {
                            ReloadInterval = TimeSpan.FromMinutes(60) // Reload interval
                        }
                    );
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception while trying to connect to Azure Key Vault");
                    Console.WriteLine(ex.Message);
                    throw;
                }
            }

            // Set up Serilog logging
            builder.Host.UseSerilog(SerilogConfiguration.CreateSerilogLogger(builder.Configuration, AppName));

            // Configure services for the application
            builder.Services.AddGatewayCustomMvc(builder.Configuration);
            builder.Services.AddGatewayApplicationServices(builder.Configuration);
            builder.Services.AddGatewayCustomIntegrations(builder.Configuration);
            builder.Services.AddHealthChecks();

            var app = builder.Build();

            // If not in development, configure for production-level settings
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error"); // Redirect to error page on exception
                app.UseHsts();                    // Apply HTTP Strict Transport Security
                app.UseHttpsRedirection();         // Force HTTPS usage
            }

            // Handle path base if configured (useful for reverse proxies)
            var pathBase = builder.Configuration["PATH_BASE"];
            if (!string.IsNullOrEmpty(pathBase))
            {
                app.UsePathBase(pathBase);
            }

            // Swagger UI configuration for Development
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API v1");
                    c.RoutePrefix = string.Empty;  // Makes Swagger available at the root URL
                });
            }

            // Configure CORS policy and routing
            app.UseCors("CorsPolicy");
            app.UseRouting();
            app.UseAuthentication();   // Enable authentication
            app.UseAuthorization();    // Enable authorization

            // Map the default controller routes
            app.MapDefaultControllerRoute();
            app.MapControllers();

            // Health check endpoints
            app.MapHealthChecks("/hc", new HealthCheckOptions()
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });
            app.MapHealthChecks("/liveness", new HealthCheckOptions
            {
                Predicate = r => r.Name.Contains("self")
            });

            // Log the current environment for troubleshooting purposes
            var env = app.Services.GetService<IWebHostEnvironment>();
            Log.Information(
                $"EnvironmentVariable: ASPNETCORE_ENVIRONMENT {envName} EnvironmentName:{env?.EnvironmentName}");

            // Run the application
            try
            {
                Log.Information("Starting Web Application ({ApplicationContext})...", AppName);
                await app.RunAsync().ConfigureAwait(true);
                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Program terminated unexpectedly ({ApplicationContext})!", AppName);
                throw;
            }
            finally
            {
                Log.CloseAndFlush();  // Ensure Serilog flushes logs before exiting
            }
        }
    }
}