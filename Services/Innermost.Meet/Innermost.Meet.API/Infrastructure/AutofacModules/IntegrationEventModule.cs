using Innermost.Meet.API.Application.IntegrationEventHandles;

namespace Innermost.Meet.API.Infrastructure.AutofacModules
{
    public class IntegrationEventModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(LifeRecordSetSharedIntegrationEventHandler).GetTypeInfo().Assembly).AsClosedTypesOf(typeof(IIntegrationEventHandler<>));
        }
    }
}
