using Innermost.LogLife.API.Application.DomainEventHandlers.LifeRecordSetShared;

namespace Innemost.LogLife.API.Infrastructure.AutofacModules
{
    /// <summary>
    /// This Autofac module is for MediatR to use IOC by Autofac.
    /// </summary>
    public class MediatRModules
        : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly)
                .AsImplementedInterfaces();

            builder.Register<ServiceFactory>(context =>
            {
                var componentContext = context.Resolve<IComponentContext>();
                return t => { object o; return componentContext.TryResolve(t, out o) ? o : null; };
            });

            builder.RegisterAssemblyTypes(typeof(CreateRecordCommand).GetTypeInfo().Assembly)
                .AsClosedTypesOf(typeof(IRequestHandler<,>));

            builder.RegisterAssemblyTypes(typeof(LifeRecordSetSharedDomainEventHandler).GetTypeInfo().Assembly).AsClosedTypesOf(typeof(INotificationHandler<>));

            builder.RegisterGeneric(typeof(TransactionBehavior<,>)).As(typeof(IPipelineBehavior<,>));
        }
    }
}
