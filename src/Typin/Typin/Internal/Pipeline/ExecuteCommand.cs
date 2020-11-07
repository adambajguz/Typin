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
    using Typin.Internal.Input;
    using Typin.OptionFallback;
    using Typin.Schemas;

    internal sealed class ExecuteCommand : IMiddleware
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IOptionFallbackProvider _optionFallbackProvider;
        private readonly ICliModeSwitcher _modeSwitcher;

        public ExecuteCommand(IServiceProvider serviceProvider,
                              IOptionFallbackProvider optionFallbackProvider,
                              ICliModeSwitcher modeSwitcher)
        {
            _serviceProvider = serviceProvider;
            _modeSwitcher = modeSwitcher;
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
            Type currentModeType = _modeSwitcher.CurrentType!;

            // Execute directives
            if (!await ProcessDefinedDirectives(context))
                return ExitCodes.Success;

            // Handle commands not supported in current mode
            if (!commandSchema.CanBeExecutedInMode(currentModeType))
                throw ModeEndUserExceptions.CommandExecutedInInvalidMode(commandSchema, currentModeType);

            // Get command instance from context and bind arguments
            ICommand instance = context.Command;
            commandSchema.Bind(instance, input, _optionFallbackProvider);

            // Execute command
            await instance.ExecuteAsync(context.Console);

            return context.ExitCode ??= ExitCodes.Success;
        }

        private async Task<bool> ProcessDefinedDirectives(ICliContext context)
        {
            Type currentModeType = _modeSwitcher.CurrentType!;
            IReadOnlyList<DirectiveInput> directives = context.Input.Directives;

            foreach (DirectiveInput directiveInput in directives)
            {
                // Try to get the directive matching the input or fallback to default
                DirectiveSchema directive = context.RootSchema.TryFindDirective(directiveInput.Name) ?? throw ArgumentBindingExceptions.UnknownDirectiveName(directiveInput);

                // Handle interactive directives not supported in current mode
                if (!directive.CanBeExecutedInMode(currentModeType))
                    throw ModeEndUserExceptions.DirectiveExecutedInInvalidMode(directive, currentModeType);

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
