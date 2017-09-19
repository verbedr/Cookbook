using AutoMapper;
using Castle.Facilities.TypedFactory;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Common.Services;
using Cookbook.Contracts.Services;
using Cookbook.Services;

namespace Cookbook.Castle
{
    public class ServicesInstaller : IWindsorInstaller
    {

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            var thisAssembly = typeof(Services.Properties.AssemblyMarker).Assembly;
            container
                .Register(
                    Classes.FromAssembly(thisAssembly)
                           .IncludeNonPublicTypes()
                           .BasedOn(typeof(IRequestHandler<,>))
                           .WithServiceFromInterface()
                           .LifestyleScoped(),
                    Component.For<IRequestHandlerFactory>().AsFactory().LifestyleSingleton(),
                    Component.For<IRequestHandlerMediator>()
                             .ImplementedBy<RequestHandlerMediator>()
                             .LifestyleSingleton(),

                    Component.For<MapperConfiguration>()
                             .Instance(new MapperConfiguration(cfg => cfg.AddProfiles(thisAssembly)))
                             .LifestyleSingleton(),
                    Component.For<IMapper>().UsingFactory<MapperConfiguration, IMapper>(f => f.CreateMapper()).LifestyleTransient(),

                    Component.For<IMetadataService>().ImplementedBy<MetadataService>().LifestyleSingleton()
                );

        }
    }
}
