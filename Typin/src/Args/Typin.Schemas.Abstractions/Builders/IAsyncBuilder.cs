namespace Typin.Schemas.Builders
{
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Represents an asynchronous builder.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IAsyncBuilder<T>
    {
        /// <summary>
        /// Builds an instance of <typeparamref name="T"/>.
        /// </summary>
        /// <returns></returns>
        Task<T> BuildAsync(CancellationToken cancellationToken);
    }
}
