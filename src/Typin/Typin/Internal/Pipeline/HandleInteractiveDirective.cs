namespace Typin.Internal.Pipeline
{
    using System.Threading;
    using System.Threading.Tasks;
    using Typin;
    using Typin.Input;
    using Typin.Internal.Exceptions;

    internal sealed class HandleInteractiveDirective : IMiddleware
    {
        public HandleInteractiveDirective()
        {

        }

        public async Task HandleAsync(ICliContext context, CommandPipelineHandlerDelegate next, CancellationToken cancellationToken)
        {
            context.ExitCode ??= Execute((CliContext)context);

            await next();
        }

        private int? Execute(CliContext context)
        {
            // Get configuration and input from context
            ApplicationConfiguration _configuration = context.Configuration;
            CommandInput input = context.Input;

            // Handle interactive directive not supported in application
            if (!_configuration.IsInteractiveModeAllowed && input.HasDirective(BuiltInDirectives.Interactive))
                throw EndUserExceptions.InteractiveModeNotSupported();

            return null;
        }
    }
}
