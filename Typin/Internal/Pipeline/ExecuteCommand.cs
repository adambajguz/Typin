namespace Typin.Internal.Pipeline
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using Typin;
    using Typin.Input;
    using Typin.Internal.Exceptions;
    using Typin.OptionFallback;
    using Typin.Schemas;

    internal sealed class ExecuteCommand : IMiddleware
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IOptionFallbackProvider _optionFallbackProvider;

        public ExecuteCommand(IServiceProvider serviceProvider, IOptionFallbackProvider optionFallbackProvider)
        {
            _serviceProvider = serviceProvider;
            _optionFallbackProvider = optionFallbackProvider;
        }

        public async Task HandleAsync(ICliContext context, CommandPipelineHandlerDelegate next, CancellationToken cancellationToken)
        {
            context.ExitCode ??= await Execute((CliContext)context);

            await next();
        }

        private async Task<int> Execute(CliContext context)
        {
            //Get input and command schema from context
            CommandInput input = context.Input;
            CommandSchema commandSchema = context.CommandSchema;

            // Execute directives
            if (!await ProcessDefinedDirectives(context))
                return ExitCodes.Success;

            // Get command instance from context and bind arguments
            ICommand instance = context.Command;
            commandSchema.Bind(instance, input, _optionFallbackProvider);

            // Execute command
            await instance.ExecuteAsync(context.Console);

            return context.ExitCode ??= ExitCodes.Success;
        }

        private async Task<bool> ProcessDefinedDirectives(ICliContext context)
        {
            bool isInteractiveMode = context.IsInteractiveMode;
            IReadOnlyList<DirectiveInput> directives = context.Input.Directives;

            foreach (DirectiveInput directiveInput in directives)
            {
                // Try to get the directive matching the input or fallback to default
                DirectiveSchema directive = context.RootSchema.TryFindDirective(directiveInput.Name) ?? throw EndUserTypinExceptions.UnknownDirectiveName(directiveInput);

                if (!isInteractiveMode && directive.InteractiveModeOnly)
                    throw EndUserTypinExceptions.InteractiveModeDirectiveNotAvailable(directiveInput.Name);

                // Get directive instance
                IDirective instance = (IDirective)_serviceProvider.GetRequiredService(directive.Type);

                await instance.HandleAsync(context.Console);

                if (!instance.ContinueExecution)
                    return false;
            }

            return true;
        }
    }
}
