//2023 (c) TD Synnex - All Rights Reserved.





namespace Configuration.API.Infrastructure.AutofacModules;

public class MediatorModule : Autofac.Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly)
            .AsImplementedInterfaces();
        //// Register all the Command classes (they implement IRequestHandler) in assembly holding the Commands
        //builder.RegisterAssemblyTypes(typeof(UserLoginCommand).GetTypeInfo().Assembly)
        //    .AsClosedTypesOf(typeof(IRequestHandler<,>));

        //builder
        //  .RegisterAssemblyTypes(typeof(UserLoginCommandValidator).GetTypeInfo().Assembly)
        //  .Where(t => t.IsClosedTypeOf(typeof(IValidator<>)))
        //  .AsImplementedInterfaces();
        //// Register the DomainEventHandler classes (they implement INotificationHandler<>) in assembly holding the Domain Events
        //builder.RegisterAssemblyTypes(typeof(OrderReceivedDomainEventHandler).GetTypeInfo().Assembly)
        //    .AsClosedTypesOf(typeof(INotificationHandler<>));

        //// Register the Command's Validators (Validators based on FluentValidation library)
        //builder
        //    .RegisterAssemblyTypes(typeof(CreatePreProvisioningOrderCommandValidator).GetTypeInfo().Assembly)
        //    .Where(t => t.IsClosedTypeOf(typeof(IValidator<>)))
        //    .AsImplementedInterfaces();

        builder.RegisterGeneric(typeof(LoggingBehavior<,>)).As(typeof(IPipelineBehavior<,>));
        builder.RegisterGeneric(typeof(ValidatorBehavior<,>)).As(typeof(IPipelineBehavior<,>));
        //builder.RegisterGeneric(typeof(TransactionBehaviour<,>)).As(typeof(IPipelineBehavior<,>));

    }
}
