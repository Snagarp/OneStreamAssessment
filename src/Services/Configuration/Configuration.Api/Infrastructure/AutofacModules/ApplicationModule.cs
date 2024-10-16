//2023 (c) TD Synnex - All Rights Reserved.

namespace Configuration.API.Infrastructure.AutofacModules;
public class ApplicationModule
    : Autofac.Module
{

    public string QueriesConnectionString { get; }

    public ApplicationModule(string qconstr)
    {
        QueriesConnectionString = qconstr;

    }

    protected override void Load(ContainerBuilder builder)
    {

        //builder.Register(c => new UserQueries(QueriesConnectionString))
        //    .As<IUserQueries>()
        //    .InstancePerLifetimeScope();

        ////builder.RegisterType<BuyerRepository>()
        ////    .As<IBuyerRepository>()
        ////    .InstancePerLifetimeScope();

        //builder.RegisterType<UserRepository>()
        //    .As<IUserRepository>()
        //    .InstancePerLifetimeScope();

        //builder.RegisterType<RequestManager>()
        //    .As<IRequestManager>()
        //    .InstancePerLifetimeScope();

        //builder.RegisterAssemblyTypes(typeof(UserLoginCommandHandler).GetTypeInfo().Assembly)
        //    .AsClosedTypesOf(typeof(IIntegrationEventHandler<>));

        //builder.RegisterAssemblyTypes(typeof(OrderErpResponseReceivedCommandHandler).GetTypeInfo().Assembly)
        //   .AsClosedTypesOf(typeof(IIntegrationEventHandler<>));

    }
}
