namespace Typin.Modes
{
    using System;
    using Microsoft.Extensions.DependencyInjection;
    using Typin.Components;
    using Typin.Schemas;

    /// <summary>
    /// <see cref="ICliMode"/> component scanner.
    /// </summary>
    internal sealed class CliModeScanner : Scanner<ICliMode>, ICliModeScanner
    {
        /// <summary>
        /// Initializes a new instance of <see cref="CliModeScanner"/>.
        /// </summary>
        /// <param name="services"></param>
        public CliModeScanner(IServiceCollection services) : base(services)
        {

        }

        /// <inheritdoc/>
        public override bool IsValidComponent(Type type)
        {
            return KnownTypesHelpers.IsCliModeType(type);
        }

        /// <inheritdoc/>
        protected override void RegisterService(Type type)
        {
            Services.AddSingleton(type);
        }
    }
}
