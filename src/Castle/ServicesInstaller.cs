using AutoMapper;
using Castle.Facilities.TypedFactory;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Common.Services;
using Cookbook.Contracts.Services;
using Cookbook.Services;
using System;

namespace Cookbook.Castle
{
    public class ServicesInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            var serviceAssembly = typeof(Services.Properties.AssemblyMarker).Assembly;
            container
                .Register(
                    Classes.FromAssembly(serviceAssembly)
                           .IncludeNonPublicTypes()
                           .BasedOn(typeof(IHandler<,>))
                           .WithServiceFromInterface()
                           .LifestyleScoped(),
                    Component.For<IHandlerFactory>().AsFactory().LifestyleSingleton(),
                    Component.For<IHandlerMediator>()
                             .ImplementedBy<HandlerMediator>()
                             .LifestyleSingleton(),

                    Component.For<MapperConfiguration>()
                             .Instance(new MapperConfiguration(cfg => cfg.AddProfiles(serviceAssembly)))
                             .LifestyleSingleton(),
                    Component.For<IMapper>().UsingFactory<MapperConfiguration, IMapper>(f => f.CreateMapper()).LifestyleTransient(),

                    Types.FromAssembly(typeof(Contracts.Properties.AssemblyMarker).Assembly)
                        .Where(t => t.IsInterface && t.Name.EndsWith("Service"))
                        .LifestyleSingleton()
                        .Configure(r => r.UsingFactoryMethod((k, m, c) =>
                            Activator.CreateInstance(ServiceBuilder.CompileResultType(c.RequestedType), k.Resolve<IHandlerMediator>())))
                );

        }
    }
}
