namespace Typin.Internal.Pipeline
{
    using System.Threading;
    using System.Threading.Tasks;
    using Typin;

    internal sealed class ExecuteCommand : IMiddleware
    {
        public async Task HandleAsync(ICliContext context, CommandPipelineHandlerDelegate next, CancellationToken cancellationToken)
        {
            // Get command instance from context
            ICommand instance = context.Command;

            // Execute command
            await instance.ExecuteAsync(context.Console);

            context.ExitCode ??= ExitCodes.Success;

            await next();
        }
    }
}
