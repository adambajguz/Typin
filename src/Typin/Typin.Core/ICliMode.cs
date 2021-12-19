namespace Typin
{
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// CLI mode definition.
    /// </summary>
    public interface ICliMode
    {
        /// <summary>
        /// Executes CLI mode.
        /// </summary>
        /// <param name="cancellationToken"></param>
        Task<int> ExecuteAsync(CancellationToken cancellationToken);
    }
}
