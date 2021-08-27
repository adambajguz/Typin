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
        /// <param name="executor"></param>
        /// <param name="isStartupContext"></param>
        /// <param name="cancellationToken"></param>
        ValueTask<int> ExecuteAsync(ICliCommandExecutor executor, bool isStartupContext, CancellationToken cancellationToken);
    }
}
