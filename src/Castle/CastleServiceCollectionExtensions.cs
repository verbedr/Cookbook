using Castle.Facilities.TypedFactory;
using Castle.Windsor;
using Cookbook.Castle;
using Microsoft.Extensions.Configuration;
using Castle.Facilities.Logging;
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
            // should happen with the option to choose the config file (func<>)
            var log4netConfigFile = configuration.GetValue<string>("Settings:Log4NetConfigFile");
            if (!string.IsNullOrWhiteSpace(log4netConfigFile))
            {
                container.AddFacility<Castle.Facilities.Logging.LoggingFacility>(f =>
                    f.LogUsing<Castle.Services.Logging.Log4netIntegration.Log4netFactory>()
                     .WithConfig(log4netConfigFile));
                // need to wire in the iloggerfactory and iloggerprovider from dotnet core so we can use 
                // https://dotnetthoughts.net/how-to-use-log4net-with-aspnetcore-for-logging/
                // but how do we route the log? castle -> dot.net core? or dot.net core to castle?
                // there is a better chance that dot.net core log is used so we should provide a logging facility for dot.net core logging. 
                // or just provide a null logger for dot.net core and mimic castle logging.
            }
            container.Kernel.Resolver.AddSubResolver(new AppSettingsConvention(configuration.GetSection("Settings")));
            container.Install(new RepositoryInstaller());
            container.Install(new DomainInstaller());
            container.Install(new ServicesInstaller());
            container.Register(Castle.MicroKernel.Registration.Component.For<IPrincipalFactory>().AsFactory().LifestyleSingleton());
            return WindsorRegistrationHelper.CreateServiceProvider(container, services);
        }
    }
}