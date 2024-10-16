namespace VendorConfiguration.Api.Infrastructure.AutofacModules
{
    using Autofac;
    using AutoMapper;

    /// <summary>
    /// This module is responsible for configuring and registering AutoMapper within the Autofac container.
    /// It sets up AutoMapper profiles and the necessary services for object mapping across the application.
    /// </summary>
    public class AutoMapperModule : Module
    {
        /// <summary>
        /// Overrides the Load method of the Autofac Module to configure the AutoMapper services.
        /// Registers AutoMapper's IConfigurationProvider and IMapper to be used throughout the application.
        /// </summary>
        /// <param name="builder">The Autofac container builder used for service registrations.</param>
        protected override void Load(ContainerBuilder builder)
        {
            // Register the AutoMapper configuration provider
            builder.Register(context =>
            {
                // Resolve all AutoMapper profiles from the container
                var profiles = context.Resolve<IEnumerable<Profile>>();

                // Create a new MapperConfiguration instance with the resolved profiles
                var config = new MapperConfiguration(x =>
                {
                    foreach (var profile in profiles)
                    {
                        x.AddProfile(profile); // Add all profiles to the configuration
                    }
                });

                return config; // Return the MapperConfiguration object
            })
            // Ensure the configuration is a singleton and initialized at application startup
            .SingleInstance()        // Register as a singleton
            .AutoActivate()          // Auto-activate to initialize at startup
            .As<IConfigurationProvider>()  // Register as IConfigurationProvider (AutoMapper's configuration)
            .As<IConfiguration>()         // Also register as IConfiguration (optional, remove if not needed)
            .AsSelf();                    // Register the MapperConfiguration itself

            // Register the IMapper instance using the resolved MapperConfiguration
            builder.Register(tempContext =>
            {
                // Resolve the Autofac component context
                var ctx = tempContext.Resolve<IComponentContext>();

                // Resolve the MapperConfiguration from the context
                var config = ctx.Resolve<MapperConfiguration>();

                // Create the IMapper instance using the configuration
                return config.CreateMapper(t => ctx.Resolve(t)); // Create IMapper instance
            })
            .As<IMapper>();  // Register as the IMapper interface

            base.Load(builder);
        }
    }
}
