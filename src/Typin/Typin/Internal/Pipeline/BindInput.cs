namespace Typin.Internal.Pipeline
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Typin;
    using Typin.Input;
    using Typin.Internal.Exceptions;
    using Typin.Internal.Input;
    using Typin.OptionFallback;
    using Typin.Schemas;

    internal sealed class BindInput : IMiddleware
    {
        private readonly IOptionFallbackProvider _optionFallbackProvider;
        private readonly ICliApplicationLifetime _applicationLifetime;

        public BindInput(IOptionFallbackProvider optionFallbackProvider,
                         ICliApplicationLifetime applicationLifetime)
        {
            _applicationLifetime = applicationLifetime;
            _optionFallbackProvider = optionFallbackProvider;
        }

        public async Task HandleAsync(ICliContext context, CommandPipelineHandlerDelegate next, CancellationToken cancellationToken)
        {
            //Get input and command schema from context
            CommandInput input = context.Input;
            CommandSchema commandSchema = context.CommandSchema;
            Type currentModeType = _applicationLifetime.CurrentModeType!;

            // Handle commands not supported in current mode
            if (!commandSchema.CanBeExecutedInMode(currentModeType))
                throw ModeEndUserExceptions.CommandExecutedInInvalidMode(commandSchema, currentModeType);

            // Get command instance from context and bind arguments
            ICommand instance = context.Command;
            commandSchema.BindParameters(instance, input.Parameters);
            commandSchema.BindOptions(instance, input.Options, _optionFallbackProvider);

            await next();
        }
    }
}
