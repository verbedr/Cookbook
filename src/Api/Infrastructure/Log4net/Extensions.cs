using Microsoft.Extensions.Logging;

namespace Cookbook.Api.Infrastructure.Log4net
{
    public static class Extensions
    {
        public static ILoggerFactory AddLog4Net(this ILoggerFactory factory, string log4NetConfigFile)
        {
            if (string.IsNullOrWhiteSpace(log4NetConfigFile)) return AddLog4Net(factory);

            factory.AddProvider(new Provider(log4NetConfigFile));
            return factory;
        }

        public static ILoggerFactory AddLog4Net(this ILoggerFactory factory)
        {
            factory.AddProvider(new Provider("log4net.config"));
            return factory;
        }
    }
}
