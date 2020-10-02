namespace Typin.Internal.Pipeline
{
    using System.Threading;
    using System.Threading.Tasks;
    using Typin;
    using Typin.Exceptions;
    using Typin.Input;
    using Typin.Internal.Exceptions;
    using Typin.Schemas;

    internal sealed class HandleInteractiveCommands : IMiddleware
    {
        public HandleInteractiveCommands()
        {

        }

        public async Task HandleAsync(ICliContext context, CommandPipelineHandlerDelegate next, CancellationToken cancellationToken)
        {
            context.ExitCode ??= Execute((CliContext)context);

            await next();
        }

        private int? Execute(CliContext context)
        {
            //Get configuration and input from context
            ApplicationConfiguration _configuration = context.Configuration;
            CommandInput input = context.Input;

            // Get command schema from context
            CommandSchema commandSchema = context.CommandSchema;

            // Handle commands not supported in normal mode
            if (!_configuration.IsInteractiveModeAllowed && commandSchema.InteractiveModeOnly)
            {
                throw EndUserTypinExceptions.InteractiveOnlyCommandButThisIsNormalApplication(commandSchema);
            }

            // Handle commands supported only in interactive mode when interactive mode was not started
            if (_configuration.IsInteractiveModeAllowed && commandSchema.InteractiveModeOnly &&
                !(context.IsInteractiveMode || input.IsInteractiveDirectiveSpecified))
            {
                throw EndUserTypinExceptions.InteractiveOnlyCommandButInteractiveModeNotStarted(commandSchema);
            }

            return null;
        }
    }
}
