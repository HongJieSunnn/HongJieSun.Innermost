using Innermost.Meet.SignalRHub.Application.IntegrationEventHandlers;
using System.Reflection;

namespace Innermost.Meet.SignalRHub.Infrastructure.AutofacModules
{
    public class IntegrationEventModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(AdminSendMessageToUserIntegrationEventHandler).GetTypeInfo().Assembly).As(typeof(IIntegrationEventHandler<>));
        }
    }
}
