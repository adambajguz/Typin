namespace Typin.Directives
{
    using System;
    using Microsoft.Extensions.DependencyInjection;
    using Typin.Components;
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
        protected override void RegisterService(Type type)
        {
            Services.AddTransient(type);
        }
    }
}
