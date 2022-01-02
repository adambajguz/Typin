namespace Typin.Pipeline
{
    using System.Threading;
    using System.Threading.Tasks;
    using PackSite.Library.Pipelining;
    using Typin;

    /// <summary>
    /// Binds input.
    /// </summary>
    public sealed class BindInput : IMiddleware
    {
        /// <summary>
        /// Initializes a new instance of <see cref="BindInput"/>.
        /// </summary>
        public BindInput()
        {

        }

        /// <inheritdoc/>
        public async ValueTask ExecuteAsync(CliContext args, StepDelegate next, IInvokablePipeline<CliContext> invokablePipeline, CancellationToken cancellationToken = default)
        {
            //Type currentModeType = _applicationLifetime.CurrentModeType!;

            //// Handle commands not supported in current mode
            //if (!commandSchema.CanBeExecutedInMode(currentModeType))
            //{
            //    throw ModeEndUserExceptions.CommandExecutedInInvalidMode(commandSchema, currentModeType);
            //}

            args.Binder.Bind(args.Services);

            await next();
        }
    }
}
