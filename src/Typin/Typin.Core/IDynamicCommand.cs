namespace Typin
{
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Entry point of a dynamic command.
    /// </summary>
    public interface IDynamicCommand
    {
        /// <summary>
        /// Executes the command with a cancellation token.
        /// This is the method that's called when the command is invoked by a user through command line.
        /// </summary>
        /// <param name="cancellationToken">Command cancellation token.</param>
        /// <remarks>If the execution of the command is not asynchronous, simply end the method with <code>return default;</code></remarks>
        ValueTask ExecuteAsync(CancellationToken cancellationToken);
    }
}