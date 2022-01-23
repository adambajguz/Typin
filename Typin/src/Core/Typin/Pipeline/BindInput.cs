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
            args.Binder.Bind(args.Services);

            await next();
        }
    }
}
