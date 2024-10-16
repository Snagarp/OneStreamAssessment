#pragma warning disable CS8601, CA1861  

using Azure.Extensions.AspNetCore.Configuration.Secrets;
using AutoMapper;
using Common.Configuration;
using FluentValidation.AspNetCore;
using Identity.Security.PingFederate;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Streamone.Integrations.BuildingBlocks.EventBus;
using Streamone.Integrations.BuildingBlocks.EventBus.Abstractions;
using Streamone.Integrations.BuildingBlocks.EventBus.EventBusServiceBus;
using Streamone.Integrations.BuildingBlocks.IntegrationEventLogEF;
using StreamOne.Integrations.BuildingBlocks.IntegrationEventLogEF.Services;
using VendorConfiguration.Api.Infrastructure.Handlers;
using VendorConfiguration.Api.Infrastructure.Mappers;
using VendorConfiguration.Application.Services;
using VendorConfiguration.Application.Validations;
using VendorConfiguration.Infrastructure;
using Microsoft.Extensions.Logging;
using System.Reflection;
using System.Data.Common;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;

namespace VendorConfiguration.Api.Configuration
{
    static class CustomExtensionsMethods
    {
        // Adds MVC with FluentValidation and CommonMvc integrations
        public static IServiceCollection AddCustomMvc(this IServiceCollection services, IConfiguration configuration)
        {
            ArgumentGuard.NotNull(configuration, nameof(configuration));

            services.AddCommonMvc(configuration); // Common MVC integrations
            services.AddValidatorsFromAssemblyContaining<CreateCountryCommandValidator>();
            services.AddFluentValidationAutoValidation();
            return services;
        }

        // Configures health checks for SQL Server and Azure Service Bus (if enabled)
        public static IServiceCollection AddHealthChecks(this IServiceCollection services, IConfiguration configuration)
        {
            ArgumentGuard.NotNull(configuration, nameof(configuration));

            var hcBuilder = services.AddHealthChecks();

            hcBuilder.AddCheck("self", () => HealthCheckResult.Healthy());

            hcBuilder
               .AddSqlServer(
                   configuration["vendorconfiguration-db-connection"]!,
                   name: "vendorconfiguration",
                   tags: new string[] { "vendorconfigurationdb" });

            if (configuration.GetValue<bool>("AzureServiceBusEnabled"))
            {
                hcBuilder
                    .AddAzureServiceBusTopic(
                        configuration["EventBusConnection"]!,
                        topicName: "s1_int_event_bus",
                        name: "vendorconfiguration-servicebus-check",
                        tags: new string[] { "servicebus" });
            }

            return services;
        }

        // Configures DbContext for the Vendor Configuration service and the IntegrationEventLog
        public static IServiceCollection AddCustomDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            ArgumentGuard.NotNull(configuration, nameof(configuration));

            var constr = configuration["vendorconfiguration-db-connection"];
            services.AddDbContext<VendorConfigurationContext>(options =>
            {
                options.UseSqlServer(constr, sqlOptions =>
                {
                    sqlOptions.MigrationsAssembly(typeof(Program).GetTypeInfo().Assembly.GetName().Name);
                    sqlOptions.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                });
            });

            services.AddDbContext<IntegrationEventLogContext>(options =>
            {
                options.UseSqlServer(constr, sqlOptions =>
                {
                    sqlOptions.MigrationsAssembly(typeof(Program).GetTypeInfo().Assembly.GetName().Name);
                    sqlOptions.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                });
            });

            return services;
        }

        // Configures Swagger for API documentation
        public static IServiceCollection AddCustomSwagger(this IServiceCollection services, IConfiguration configuration)
        {
            ArgumentGuard.NotNull(configuration, nameof(configuration));

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "OneStream Enterprise - Vendor Configuration REST API ( Private Core APIs for internal Web applications)",
                    Version = "v1",
                    Description = "The Vendor Configuration Service REST API"
                });

                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });

            return services;
        }

        // Configures integrations (PingFederate, Azure Key Vault, etc.)
        public static IServiceCollection AddCustomIntegrations(this IServiceCollection services, IConfiguration configuration)
        {
            ArgumentGuard.NotNull(configuration, nameof(configuration));

            services.AddCommonIntegrations(configuration);
            services.AddPingFederateAuthentication(configuration);

            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new ConfigurationProfile());
            });
            var mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);

            services.AddSingleton<IKeyVault, KeyVaultUtility>();
            services.AddScoped<HttpClientLoggingHandler>();

            services.AddTransient<Func<DbConnection, IIntegrationEventLogService>>(sp =>
                (DbConnection c) => new IntegrationEventLogService(c));

            if (configuration.GetValue<bool>("AzureServiceBusEnabled"))
            {
                services.AddSingleton<IServiceBusPersisterConnection>(sp =>
                {
                    var serviceBusConnectionString = configuration["EventBusConnection"];
                    return new DefaultServiceBusPersisterConnection(serviceBusConnectionString);
                });
            }

            return services;
        }

        // Configures EventBus with Azure ServiceBus support (if enabled)
        public static IServiceCollection AddEventBus(this IServiceCollection services, IConfiguration configuration)
        {
            ArgumentGuard.NotNull(configuration, nameof(configuration));

            if (configuration.GetValue<bool>("AzureServiceBusEnabled"))
            {
                services.AddSingleton<IEventBus, EventBusServiceBus>(sp =>
                {
                    var serviceBusPersisterConnection = sp.GetRequiredService<IServiceBusPersisterConnection>();
                    var logger = sp.GetRequiredService<ILogger<EventBusServiceBus>>();
                    var eventBusSubcriptionsManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();
                    string? subscriptionName = configuration["SubscriptionClientName"];
                    string? topicName = configuration["EventBusTopic"];

                    return new EventBusServiceBus(serviceBusPersisterConnection, logger,
                        eventBusSubcriptionsManager, sp, subscriptionName, topicName);
                });

                services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();
            }

            return services;
        }
    }
}