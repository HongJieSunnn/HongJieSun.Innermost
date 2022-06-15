using Autofac;
using CommonService.Behaviors;
using MediatR;
using MediatR.Extensions.Autofac.DependencyInjection;
using TagS.Microservices.Client.AutofacExtensions;

namespace Innermost.MusicHub.API.Infrastructure.AutofacModules
{
    public class MediatRModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterMediatR(typeof(Program).Assembly);

            builder.RegisterGeneric(typeof(MongoDBTransactionBehavior<,>)).As(typeof(IPipelineBehavior<,>));

            builder.RegisterTagSMicroservicesClientTypes();
        }
    }
}
