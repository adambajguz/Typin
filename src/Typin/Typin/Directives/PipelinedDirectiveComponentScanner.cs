namespace Typin.Directives
{
    using System;
    using Microsoft.Extensions.DependencyInjection;
    using Typin.Components;
    using Typin.Schemas;

    /// <summary>
    /// <see cref="IPipelinedDirective"/> component scanner.
    /// </summary>
    public sealed class PipelinedDirectiveComponentScanner : ComponentScanner<IPipelinedDirective>
    {
        /// <summary>
        /// Initializes a new instance of <see cref="PipelinedDirectiveComponentScanner"/>.
        /// </summary>
        /// <param name="services"></param>
        public PipelinedDirectiveComponentScanner(IServiceCollection services) : base(services)
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
            Services.AddTransient(typeof(IPipelinedDirective), (provider) => provider.GetRequiredService(type));
        }
    }
}
