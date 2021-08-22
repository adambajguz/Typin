namespace Typin.Internal.Pipeline
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using PackSite.Library.Pipelining;
    using Typin;
    using Typin.Input;
    using Typin.Internal.Exceptions;
    using Typin.Schemas;

    internal sealed class InitializeDirectives : IMiddleware
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ICliApplicationLifetime _applicationLifetime;

        public InitializeDirectives(IServiceProvider serviceProvider,
                                    ICliApplicationLifetime applicationLifetime)
        {
            _serviceProvider = serviceProvider;
            _applicationLifetime = applicationLifetime;
        }

        public async ValueTask ExecuteAsync(ICliContext args, StepDelegate next, IInvokablePipeline<ICliContext> invokablePipeline, CancellationToken cancellationToken = default)
        {
            var context = args;

            //Get current CLI mode and input directives
            Type currentModeType = _applicationLifetime.CurrentModeType!;
            IReadOnlyList<DirectiveInput> directives = context.Input.Directives;

            //Initialize collections
            List<IDirective> directivesInstances = new();
            List<IPipelinedDirective> pipelinedDirectivesInstances = new();

            //Process directive input
            foreach (DirectiveInput directiveInput in directives)
            {
                // Try to get the directive matching the input or fallback to default
                DirectiveSchema directive = args.RootSchema.TryFindDirective(directiveInput.Name) ?? throw ArgumentBindingExceptions.UnknownDirectiveName(directiveInput);

                // Handle interactive directives not supported in current mode
                if (!directive.CanBeExecutedInMode(currentModeType))
                {
                    throw ModeEndUserExceptions.DirectiveExecutedInInvalidMode(directive, currentModeType);
                }

                // Get directive instance
                IDirective instance = (IDirective)_serviceProvider.GetRequiredService(directive.Type);

                //Initialize directive
                await instance.OnInitializedAsync(cancellationToken);

                //Add directive to list
                directivesInstances.Add(instance);

                if (directive.IsPipelinedDirective && instance is IPipelinedDirective pd)
                {
                    pipelinedDirectivesInstances.Add(pd);
                }
            }

            //Set directives lists in context
            CliContext internalCliContext = (CliContext)context;
            internalCliContext.Directives = directivesInstances;
            internalCliContext.PipelinedDirectives = pipelinedDirectivesInstances;

            await next();
        }
    }
}
