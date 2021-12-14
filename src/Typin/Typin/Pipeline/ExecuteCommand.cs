namespace Typin.Pipeline
{
    using System.Threading;
    using System.Threading.Tasks;
    using PackSite.Library.Pipelining;
    using Typin;

    /// <summary>
    /// Executes command
    /// </summary>
    public sealed class ExecuteCommand : IMiddleware
    {
        /// <summary>
        /// Initializes a new instance of <see cref="ExecuteCommand"/>.
        /// </summary>
        public ExecuteCommand()
        {

        }

        /// <inheritdoc/>
        public async ValueTask ExecuteAsync(CliContext args, StepDelegate next, IInvokablePipeline<CliContext> invokablePipeline, CancellationToken cancellationToken = default)
        {
            // Get command instance from context
            ICommand instance = args.Command.Instance;

            // Execute command
            await instance.ExecuteAsync(cancellationToken);
            args.Output.ExitCode ??= ExitCode.Success;

            await next();
        }
    }
}
