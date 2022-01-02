namespace Typin.Plugins.Scopes
{
    using System.Threading;
    using System.Threading.Tasks;
    using PackSite.Library.Pipelining;

    /// <summary>
    /// A middleware that handles scopes.
    /// </summary>
    public sealed class ScopeHandler : IMiddleware
    {
        /// <summary>
        /// Initializes a new instance of <see cref="ScopeHandler"/>.
        /// </summary>
        public ScopeHandler()
        {

        }

        /// <inheritdoc/>
        public async ValueTask ExecuteAsync(CliContext args, StepDelegate next, IInvokablePipeline<CliContext> invokablePipeline, CancellationToken cancellationToken = default)
        {
            await next();
        }
    }
}
