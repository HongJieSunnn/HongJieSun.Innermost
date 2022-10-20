using System.Reflection;

namespace Innermost.TagServer.API.Infrastructure.AutofacModules
{
    internal class MediatRModule : Autofac.Module//TODO there is the dependencyInjection lib writed by MediatR official.
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

            builder.RegisterTagSMicroservicesServerTypes();
        }
    }
}
