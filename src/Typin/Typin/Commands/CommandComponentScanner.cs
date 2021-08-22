namespace Typin.Commands
{
    using System;
    using Microsoft.Extensions.DependencyInjection;
    using Typin.Components;
    using Typin.Schemas;

    /// <summary>
    /// <see cref="ICommand"/> component scanner.
    /// </summary>
    public sealed class CommandComponentScanner : ComponentScanner<ICommand>
    {
        /// <summary>
        /// Initializes a new instance of <see cref="CommandComponentScanner"/>.
        /// </summary>
        /// <param name="services"></param>
        public CommandComponentScanner(IServiceCollection services) : base(services)
        {

        }

        /// <inheritdoc/>
        public override bool IsValidComponent(Type type)
        {
            return KnownTypesHelpers.IsCommandType(type);
        }

        /// <inheritdoc/>
        protected override void RegisterService(Type type)
        {
            Services.AddScoped(type);
            Services.AddScoped(typeof(ICommand), (provider) => provider.GetRequiredService(type));
        }
    }
}
