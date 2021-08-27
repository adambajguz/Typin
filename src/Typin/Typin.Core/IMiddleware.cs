namespace Typin
{
    using PackSite.Library.Pipelining;

    /// <summary>
    /// Pipeline middleware to surround the inner handler.
    /// Implementations add additional behavior and await the next delegate.
    /// </summary>
    public interface IMiddleware : IStep<CliContext>
    {

    }
}
