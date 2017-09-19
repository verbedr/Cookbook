using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Common.Domain;
using Cookbook.Domain;

namespace Cookbook.Castle
{
    public class DomainInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            var thisAssembly = typeof(Domain.Properties.AssemblyMarker).Assembly;
            container
                .Register(
                    Classes.FromAssembly(thisAssembly)
                           .IncludeNonPublicTypes()
                           .BasedOn(typeof(IQuery<,>))
                           .WithServiceFromInterface()
                           .LifestyleScoped(),
                    Component.For<CookbookFactory>().LifestyleSingleton());
        }
    }
}
