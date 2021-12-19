namespace Typin.Directives
{
    using System;
    using Microsoft.Extensions.DependencyInjection;
    using Typin.Components;
    using Typin.Exceptions.Resolvers.DirectiveResolver;
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
        protected override Exception GetInvalidComponentException(Type type)
        {
            return new InvalidPipelinedDirectiveException(type);
        }

        /// <inheritdoc/>
        protected override void RegisterServices(Type type)
        {
            Services.AddTransient(type);
        }
    }
}
