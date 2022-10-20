using MediatR.Extensions.Autofac.DependencyInjection;

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
