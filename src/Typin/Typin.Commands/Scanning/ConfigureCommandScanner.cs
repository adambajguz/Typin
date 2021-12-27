namespace Typin.Commands.Scanning
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Microsoft.Extensions.DependencyInjection;
    using Typin.Hosting.Scanning;

    /// <summary>
    /// <see cref="IConfigureCommand"/> component scanner.
    /// </summary>
    internal sealed class ConfigureCommandScanner : Scanner<IConfigureCommand>, IConfigureCommandScanner
    {
        private readonly IServiceCollection _services;

        /// <summary>
        /// Initializes a new instance of <see cref="ConfigureCommandScanner"/>.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="current"></param>
        public ConfigureCommandScanner(IServiceCollection services, IEnumerable<Type>? current) :
            base(current)
        {
            _services = services;

            Added += (sender, e) =>
            {
                _services.AddTransient(e.Type);
            };
        }

        /// <inheritdoc/>
        public override bool IsValidComponent(Type type)
        {
            return IConfigureCommand.IsValidType(type);
        }

        protected override IEnumerable<Type> GetTypes(Assembly assembly)
        {
            return assembly.ExportedTypes
                .SelectMany(x => x
                    .GetNestedTypes(BindingFlags.Public | BindingFlags.NonPublic)
                    .Where(IsValidComponent)
                )
                .Concat(assembly.ExportedTypes.Where(IsValidComponent));
        }


        /// <inheritdoc/>
        public IConfigureCommandScanner FromNested(Type type)
        {
            IEnumerable<Type>? types = type
                .GetNestedTypes(BindingFlags.Public | BindingFlags.NonPublic)
                .Where(IsValidComponent);

            if (types is not null)
            {
                Multiple(types);
            }

            return this;
        }
    }
}
