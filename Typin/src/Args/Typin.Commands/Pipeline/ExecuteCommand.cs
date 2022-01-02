namespace Typin.Commands.Pipeline
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using PackSite.Library.Pipelining;
    using Typin.Commands;
    using Typin.Commands.Features;

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
            ICommandFeature commandFeature = args.Features.Get<ICommandFeature>() ??
                throw new InvalidOperationException($"{nameof(ICommandFeature)} has not been configured for this application or call.");

            ICommand instance = commandFeature.Instance;
            ICommandHandler handlerInstance = commandFeature.HandlerInstance;

            // Execute command
            await handlerInstance.ExecuteAsync(instance, cancellationToken);
            args.Output.ExitCode ??= ExitCode.Success;

            await next();
        }
    }
}
