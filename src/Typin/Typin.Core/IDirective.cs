namespace Typin
{
    using System.Threading;
    using System.Threading.Tasks;
    using Typin.Console;

    /// <summary>
    /// Directive handler.
    /// </summary>
    public interface IDirective
    {
        /// <summary>
        /// Executes the handler using the specified implementation of <see cref="IConsole"/>.
        /// This is the method that's called when a directive is specified by a user through command line.
        /// </summary>
        /// <remarks>
        /// If the execution of the initialization method is not asynchronous, simply end the method with <code>return default;</code>.
        /// If you want to stop the execution of the command, simply throw DirectiveException.
        /// </remarks>
        ValueTask OnInitializedAsync(CancellationToken cancellationToken);
    }
}