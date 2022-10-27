using Innermost.Architecture.Consistency.MongoDB;
using MediatR.Extensions.Autofac.DependencyInjection;
using TagS.Microservices.Client.AutofacExtensions;

namespace Innermost.Meet.API.Infrastructure.AutofacModules
{
    internal class MediatRModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterMediatR(typeof(Program).Assembly);

            builder.RegisterGeneric(typeof(MongoDBTransactionBehavior<,>)).As(typeof(IPipelineBehavior<,>));

            builder.RegisterTagSMicroservicesClientTypes();
        }
    }
}
