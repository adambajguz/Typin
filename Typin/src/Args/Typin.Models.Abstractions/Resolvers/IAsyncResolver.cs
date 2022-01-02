namespace Typin.Models.Resolvers
{
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Represents an asynchronous resolver.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IAsyncResolver<T>
    {
        /// <summary>
        /// Resolves an instance of <typeparamref name="T"/>.
        /// </summary>
        /// <returns></returns>
        Task<T> ResolveAsync(CancellationToken cancellationToken);
    }
}
