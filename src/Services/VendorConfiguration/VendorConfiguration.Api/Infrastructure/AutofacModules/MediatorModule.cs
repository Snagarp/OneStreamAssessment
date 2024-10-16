using Common.Behaviors;
using VendorConfiguration.Application.Queries;

namespace VendorConfiguration.Api.Infrastructure.AutofacModules
{
    /// <summary>
    /// This module is responsible for configuring MediatR and its behaviors within the Autofac container.
    /// It registers MediatR handlers and pipeline behaviors such as logging and validation.
    /// </summary>
    public class MediatorModule : Autofac.Module
    {
        /// <summary>
        /// Overrides the Load method of the Autofac Module to configure MediatR services and pipeline behaviors.
        /// Registers command/query handlers and associated behaviors like logging and validation.
        /// </summary>
        /// <param name="builder">The Autofac container builder used for service registrations.</param>
        protected override void Load(ContainerBuilder builder)
        {
            // Guard clause to ensure the builder is not null
            ArgumentGuard.NotNull(builder, nameof(builder));

            // Register all MediatR handler types (classes implementing IRequestHandler) from the MediatR assembly
            builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly)
                .AsImplementedInterfaces();

            // Register all the Query classes (they implement IRequestHandler<,>) in the assembly holding the Queries
            builder.RegisterAssemblyTypes(typeof(CountryQuery).GetTypeInfo().Assembly)
                .AsClosedTypesOf(typeof(IRequestHandler<,>));

            // Register logging behavior in the MediatR pipeline
            builder.RegisterGeneric(typeof(LoggingBehavior<,>))
                .As(typeof(IPipelineBehavior<,>));

            // Register validation behavior in the MediatR pipeline
            builder.RegisterGeneric(typeof(ValidatorBehavior<,>))
                .As(typeof(IPipelineBehavior<,>));
        }
    }
}
