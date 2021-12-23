namespace Typin.Commands
{
    using System;
    using Microsoft.Extensions.DependencyInjection;
    using Typin.Components;
    using Typin.Exceptions.Resolvers.CommandResolver;
    using Typin.Schemas;

    /// <summary>
    /// <see cref="ICommandTemplate"/> component scanner.
    /// </summary>
    internal sealed class DynamicCommandScanner : Scanner<ICommandTemplate>, IDynamicCommandScanner
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
            return KnownTypesHelpers.IsCommandTemplateType(type);
        }

        /// <inheritdoc/>
        protected override Exception GetInvalidComponentException(Type type)
        {
            return new InvalidDynamicCommandException(type);
        }

        /// <inheritdoc/>
        protected override void RegisterServices(Type type)
        {
            Services.AddScoped(type);
        }
    }
}
