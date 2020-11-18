namespace Typin
{
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Pipeline middleware to surround the inner handler.
    /// Implementations add additional behavior and await the next delegate.
    /// </summary>
    public interface IMiddleware
    {
        /// <summary>
        /// Executes the middleware handler using the curent instance of <see cref="ICliContext"/>.
        /// Perform any additional behavior and await the next delegate as necessary.
        /// </summary>
        Task HandleAsync(ICliContext context, CommandPipelineHandlerDelegate next, CancellationToken cancellationToken);
    }
}
