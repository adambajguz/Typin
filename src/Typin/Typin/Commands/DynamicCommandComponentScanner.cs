namespace Typin.Commands
{
    using System;
    using Microsoft.Extensions.DependencyInjection;
    using Typin.Components;
    using Typin.Schemas;

    /// <summary>
    /// <see cref="IDynamicCommand"/> component scanner.
    /// </summary>
    public sealed class DynamicCommandComponentScanner : ComponentScanner<IDynamicCommand>
    {
        /// <summary>
        /// Initializes a new instance of <see cref="CommandComponentScanner"/>.
        /// </summary>
        /// <param name="services"></param>
        public DynamicCommandComponentScanner(IServiceCollection services) : base(services)
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
            Services.AddScoped(typeof(IDynamicCommand), (provider) => provider.GetRequiredService(type));
        }
    }
}
