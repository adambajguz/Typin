namespace Typin.Commands
{
    using System;
    using Microsoft.Extensions.DependencyInjection;
    using Typin.Components;
    using Typin.Schemas;

    /// <summary>
    /// <see cref="IDynamicCommand"/> component scanner.
    /// </summary>
    internal sealed class DynamicCommandScanner : Scanner<IDynamicCommand>, IDynamicCommandScanner
    {
        /// <summary>
        /// Initializes a new instance of <see cref="CommandScanner"/>.
        /// </summary>
        /// <param name="services"></param>
        public DynamicCommandScanner(IServiceCollection services) : base(services)
        {

        }

        /// <inheritdoc/>
        public override bool IsValidComponent(Type type)
        {
            return KnownTypesHelpers.IsDynamicCommandType(type);
        }

        /// <inheritdoc/>
        protected override void RegisterService(Type type)
        {
            Services.AddScoped(type);
        }
    }
}
