using System.Reflection;

namespace Innermost.Push.API.Infrastructure.AutofacModules
{
    public class IntegrationEventModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {

            builder.RegisterAssemblyTypes(typeof(PushMessageToUserIntegrationEvent).GetTypeInfo().Assembly).AsClosedTypesOf(typeof(IIntegrationEventHandler<>));
        }
    }
}
