using Innermost.LogLife.API.Application.DomainEventHandlers.LifeRecordSetShared;

namespace Innermost.LogLife.API.Infrastructure.AutofacModules
{
    public class IntegrationEventModules : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {

            builder.RegisterAssemblyTypes(typeof(LifeRecordSetSharedDomainEventHandler).GetTypeInfo().Assembly).AsClosedTypesOf(typeof(INotificationHandler<>));
        }
    }
}
