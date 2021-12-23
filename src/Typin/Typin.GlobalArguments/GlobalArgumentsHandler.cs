namespace Typin.GlobalArguments
{
    using System.Threading;
    using System.Threading.Tasks;
    using PackSite.Library.Pipelining;

    /// <summary>
    /// A middleware that handles help.
    /// </summary>
    public sealed class GlobalArgumentsHandler : IMiddleware
    {
        /// <summary>
        /// Initializes a new instance of <see cref="GlobalArgumentsHandler"/>.
        /// </summary>
        public GlobalArgumentsHandler()
        {

        }

        /// <inheritdoc/>
        public async ValueTask ExecuteAsync(CliContext args, StepDelegate next, IInvokablePipeline<CliContext> invokablePipeline, CancellationToken cancellationToken = default)
        {
            await next();
        }
    }
}
