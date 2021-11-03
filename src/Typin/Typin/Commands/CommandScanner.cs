namespace Typin.Commands
{
    using System;
    using Microsoft.Extensions.DependencyInjection;
    using Typin.Components;
    using Typin.Schemas;

    /// <summary>
    /// <see cref="ICommand"/> component scanner.
    /// </summary>
    internal sealed class CommandScanner : Scanner<ICommand>, ICommandScanner
    {
        /// <summary>
        /// Initializes a new instance of <see cref="CommandScanner"/>.
        /// </summary>
        /// <param name="services"></param>
        public CommandScanner(IServiceCollection services) : base(services)
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
        }
    }
}
