using Castle.Facilities.TypedFactory;
using Castle.Windsor;
using Cookbook.Castle;
using Microsoft.Extensions.Configuration;
using Castle.Windsor.MsDependencyInjection;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class CastleServiceCollectionExtensions
    {
        public static IServiceProvider ConfigureDependencyInjection(this IServiceCollection services, IConfigurationRoot configuration)
        {
            var container = new WindsorContainer();
            container.AddFacility<TypedFactoryFacility>();
            container.Kernel.Resolver.AddSubResolver(new AppSettingsConvention(configuration.GetSection("Settings")));
            container.Install(new RepositoryInstaller());
            container.Install(new DomainInstaller());
            container.Install(new ServicesInstaller());
            container.Register(Castle.MicroKernel.Registration.Component.For<IPrincipalFactory>().AsFactory().LifestyleSingleton());
            return WindsorRegistrationHelper.CreateServiceProvider(container, services);
        }
    }
}