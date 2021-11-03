namespace Typin.Directives
{
    using System;
    using Microsoft.Extensions.DependencyInjection;
    using Typin.Components;
    using Typin.Schemas;

    /// <summary>
    /// <see cref="IPipelinedDirective"/> component scanner.
    /// </summary>
    internal sealed class PipelinedDirectiveScanner : Scanner<IPipelinedDirective>, IPipelinedDirectiveScanner
    {
        /// <summary>
        /// Initializes a new instance of <see cref="PipelinedDirectiveScanner"/>.
        /// </summary>
        /// <param name="services"></param>
        public PipelinedDirectiveScanner(IServiceCollection services) : base(services)
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
