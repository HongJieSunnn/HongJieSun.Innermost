﻿using CommonService.Behaviors;
using MediatR.Extensions.Autofac.DependencyInjection;
using TagS.Microservices.Client.AutofacExtensions;

namespace Innermost.Meet.SignalRHub.Infrastructure.AutofacModules
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
