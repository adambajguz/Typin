namespace Typin.Internal.Pipeline
{
    using System.Threading;
    using System.Threading.Tasks;
    using PackSite.Library.Pipelining;
    using Typin;

    internal sealed class ExecuteCommand : IMiddleware
    {
        public async ValueTask ExecuteAsync(ICliContext args, StepDelegate next, IInvokablePipeline<ICliContext> invokablePipeline, CancellationToken cancellationToken = default)
        {
            var context = args;

            // Get command instance from context
            ICommand instance = context.Command;

            // Execute command
            await instance.ExecuteAsync(cancellationToken);

            context.ExitCode ??= ExitCodes.Success;

            await next();
        }
    }
}
