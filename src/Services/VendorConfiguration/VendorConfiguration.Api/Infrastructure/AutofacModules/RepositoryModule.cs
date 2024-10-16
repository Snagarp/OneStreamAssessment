using VendorConfiguration.Domain.AggregatesModel.CountryAggregate;
using VendorConfiguration.Infrastructure.Repositories;

namespace VendorConfiguration.Api.Infrastructure.AutofacModules
{
    /// <summary>
    /// This module is responsible for registering repository classes in the Autofac container.
    /// It registers repository interfaces and their concrete implementations for dependency injection.
    /// </summary>
    public class RepositoryModule : Autofac.Module
    {
        /// <summary>
        /// Overrides the Load method of the Autofac Module to register repository services.
        /// Registers repositories like <see cref="CountryRepository"/> and binds them to their respective interfaces.
        /// </summary>
        /// <param name="builder">The Autofac container builder used for service registrations.</param>
        protected override void Load(ContainerBuilder builder)
        {
            // Guard clause to ensure the builder is not null
            ArgumentGuard.NotNull(builder, nameof(builder));

            // Register CountryRepository as ICountryRepository, scoped to the lifetime of the request
            builder.RegisterType<CountryRepository>()
                .As<ICountryRepository>()
                .InstancePerLifetimeScope();
        }
    }
}
