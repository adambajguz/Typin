namespace Typin
{
    using System.Threading;
    using System.Threading.Tasks;
    using Typin.Console;
    using Typin.Models;

    /// <summary>
    /// Entry point in a command line application.
    /// </summary>
    public interface ICommand : IModel
    {
        /// <summary>
        /// Executes the command using the specified implementation of <see cref="IConsole"/>.
        /// This is the method that's called when the command is invoked by a user through command line.
        /// </summary>
        /// <remarks>If the execution of the command is not asynchronous, simply end the method with <code>return default;</code></remarks>
        ValueTask ExecuteAsync(CancellationToken cancellationToken);
    }
}