namespace Typin.Directives
{
    using System;
    using Microsoft.Extensions.DependencyInjection;
    using Typin.Components;
    using Typin.Exceptions.Resolvers.DirectiveResolver;
    using Typin.Schemas;

    /// <summary>
    /// <see cref="IDirective"/> component scanner.
    /// </summary>
    internal sealed class DirectiveScanner : Scanner<IDirective>, IDirectiveScanner
    {
        /// <summary>
        /// Initializes a new instance of <see cref="DirectiveScanner"/>.
        /// </summary>
        /// <param name="services"></param>
        public DirectiveScanner(IServiceCollection services) : base(services)
        {

        }

        /// <inheritdoc/>
        public override bool IsValidComponent(Type type)
        {
            return KnownTypesHelpers.IsDirectiveType(type);
        }

        /// <inheritdoc/>
        protected override Exception GetInvalidComponentException(Type type)
        {
            return new InvalidDirectiveException(type);
        }

        /// <inheritdoc/>
        protected override void RegisterServices(Type type)
        {
            Services.AddTransient(type);
        }
    }
}
