namespace Typin
{
    using System.Threading.Tasks;
    using PackSite.Library.Pipelining;

    /// <summary>
    /// Represents an async continuation for the next task to execute in the pipeline.
    /// </summary>
    /// <returns>Awaitable task</returns>
    public delegate ValueTask CommandPipelineHandlerDelegate();

    /// <summary>
    /// Pipeline middleware to surround the inner handler.
    /// Implementations add additional behavior and await the next delegate.
    /// </summary>
    public interface IMiddleware : IStep<ICliContext>
    {

    }
}
