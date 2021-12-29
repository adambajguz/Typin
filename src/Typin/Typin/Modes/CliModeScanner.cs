namespace Typin.Modes
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Extensions.DependencyInjection;
    using Typin.Hosting.Scanning;

    /// <summary>
    /// <see cref="ICliMode"/> component scanner.
    /// </summary>
    internal sealed class CliModeScanner : Scanner<ICliMode>, ICliModeScanner
    {
        private readonly IServiceCollection _services;

        /// <summary>
        /// Initializes a new instance of <see cref="CliModeScanner"/>.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="current"></param>
        public CliModeScanner(IServiceCollection services, IEnumerable<Type>? current) :
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
            return ICliMode.IsValidType(type);
        }
    }
}
