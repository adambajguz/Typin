namespace Typin.Directives
{
    using System;
    using Microsoft.Extensions.DependencyInjection;
    using Typin.Components;
    using Typin.Schemas;

    /// <summary>
    /// <see cref="IDirective"/> component scanner.
    /// </summary>
    public sealed class DirectiveComponentScanner : ComponentScanner<IDirective>
    {
        /// <summary>
        /// Initializes a new instance of <see cref="DirectiveComponentScanner"/>.
        /// </summary>
        /// <param name="services"></param>
        public DirectiveComponentScanner(IServiceCollection services) : base(services)
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
            Services.AddTransient(typeof(IDirective), (provider) => provider.GetRequiredService(type));
        }
    }
}
