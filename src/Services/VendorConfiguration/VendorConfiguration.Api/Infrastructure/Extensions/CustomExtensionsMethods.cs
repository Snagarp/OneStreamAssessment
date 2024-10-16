using System.Text.Json.Serialization;
using AutoMapper;
using Common.Attributes;
using Common.Configuration;
using Common.Filters;
using HybridModelBinding;
using Identity.Security.PingFederate;
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

namespace VendorConfiguration.Api.Infrastructure.Extensions
{
    /// <summary>
    /// Provides custom extension methods to configure services, DbContext, Swagger, and integrations for the application.
    /// </summary>
    static class CustomExtensionsMethods
    {
        /// <summary>
        /// Configures MVC services with custom settings, including JSON options, CORS policy, and model validation.
        /// </summary>
        /// <param name="services">The service collection to add services to.</param>
        /// <param name="configuration">The application's configuration.</param>
        /// <returns>The updated service collection.</returns>
        public static IServiceCollection AddConfigurationMvc(this IServiceCollection services, IConfiguration configuration)
        {
            switch (configuration)
            {
                case null:
                    throw new ArgumentNullException(nameof(configuration));
            }

            services.AddControllers(options =>
            {
                options.Filters.Add(typeof(HttpGlobalExceptionFilter)); // Error Handling
                options.Filters.Add(new BenchmarkAttribute());          // Adds elapsed time header to responses
            })
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.WriteIndented = true;
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                })
                .AddXmlSerializerFormatters()
                .AddHybridModelBinder();

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder
                        .SetIsOriginAllowed((host) => true)
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
            });

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressInferBindingSourcesForParameters = false;
                options.SuppressModelStateInvalidFilter = true;
            });

            services.AddValidatorsFromAssemblyContaining<CreateCountryCommandValidator>();

            return services;
        }

        /// <summary>
        /// Configures the database contexts for the application, including the main application context and integration event log context.
        /// </summary>
        /// <param name="services">The service collection to add services to.</param>
        /// <param name="configuration">The application's configuration.</param>
        /// <returns>The updated service collection.</returns>
        public static IServiceCollection AddCustomDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            var constr = configuration["vendorconfiguration-db-connection"];
            services.AddDbContext<VendorConfigurationContext>(options =>
            {
                options.UseSqlServer(constr,
                    sqlServerOptionsAction: sqlOptions =>
                    {
                        sqlOptions.MigrationsAssembly(typeof(Program).GetTypeInfo().Assembly.GetName().Name);
                        sqlOptions.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                    });
            },
                ServiceLifetime.Scoped);

            services.AddDbContext<IntegrationEventLogContext>(options =>
            {
                options.UseSqlServer(constr,
                    sqlServerOptionsAction: sqlOptions =>
                    {
                        sqlOptions.MigrationsAssembly(typeof(Program).GetTypeInfo().Assembly.GetName().Name);
                        sqlOptions.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                    });
            });

            return services;
        }

        /// <summary>
        /// Configures Swagger documentation for the application.
        /// </summary>
        /// <param name="services">The service collection to add services to.</param>
        /// <param name="configuration">The application's configuration.</param>
        /// <returns>The updated service collection.</returns>
        public static IServiceCollection AddCustomSwagger(this IServiceCollection services, IConfiguration configuration)
        {
            switch (configuration)
            {
                case null:
                    throw new ArgumentNullException(nameof(configuration));
                default:
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
        }

        /// <summary>
        /// Adds custom integrations such as Key Vault, Ping Federate authentication, and HTTP logging.
        /// </summary>
        /// <param name="services">The service collection to add services to.</param>
        /// <param name="configuration">The application's configuration.</param>
        /// <returns>The updated service collection.</returns>
        public static IServiceCollection AddCustomIntegrations(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCommonIntegrations(configuration);
            services.AddPingFederateAuthentication(configuration);

            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new ConfigurationProfile());
            });
            var mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);

            services.AddSingleton<IKeyVault, KeyVaultUtility>();
            services.AddScoped(typeof(HttpClientLoggingHandler));

            services.AddTransient<Func<DbConnection, IIntegrationEventLogService>>(
                sp => (DbConnection c) => new IntegrationEventLogService(c));

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

        /// <summary>
        /// Adds event bus services and configurations, including support for Azure Service Bus if enabled.
        /// </summary>
        /// <param name="services">The service collection to add services to.</param>
        /// <param name="configuration">The application's configuration.</param>
        /// <returns>The updated service collection.</returns>
        public static IServiceCollection AddEventBus(this IServiceCollection services, IConfiguration configuration)
        {
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
            }

            services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();

            return services;
        }
    }
}