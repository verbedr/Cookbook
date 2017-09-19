using Castle.Core;
using Castle.MicroKernel;
using Castle.MicroKernel.Context;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Cookbook.Castle
{
    internal class AppSettingsConvention : ISubDependencyResolver
    {
        private readonly IConfigurationSection _configurationSection;
        private readonly IDictionary<string, string> _knownValues;

        public AppSettingsConvention(IConfigurationSection configurationSection)
        {
            _configurationSection = configurationSection ?? throw new ArgumentNullException(nameof(configurationSection));
            _knownValues = _configurationSection.AsEnumerable().ToDictionary(x => x.Key.ToUpperInvariant(), x => x.Value);
        }

        public bool CanResolve(
            CreationContext context,
            ISubDependencyResolver contextHandlerResolver,
            ComponentModel model,
            DependencyModel dependency)
        {
            return _knownValues.ContainsKey(dependency.DependencyKey.ToUpperInvariant())
                && TypeDescriptor
                    .GetConverter(dependency.TargetType)
                    .CanConvertFrom(typeof(string));
        }

        public object Resolve(
            CreationContext context,
            ISubDependencyResolver contextHandlerResolver,
            ComponentModel model,
            DependencyModel dependency)
        {
            return TypeDescriptor
                .GetConverter(dependency.TargetType)
                .ConvertFrom(
                    _knownValues[dependency.DependencyKey.ToUpperInvariant()]);
        }
    }
}
