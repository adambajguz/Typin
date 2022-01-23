namespace Typin.Commands
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Typin.Modes;

    /// <summary>
    /// CLI command executor.
    /// A scope is defined as a lifetime of a command execution pipeline that includes directives handling.
    /// </summary>
    public interface ICommandExecutor
    {
        /// <summary>
        /// Executes a command.
        /// </summary>
        /// <param name="arguments"></param>
        /// <param name="inputOptions"></param>
        /// <param name="options"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<int> ExecuteAsync(IEnumerable<string> arguments,
                               InputOptions inputOptions = default,
                               ModeBehavior options = default,
                               CancellationToken cancellationToken = default);

        /// <summary>
        /// Executes a command.
        /// </summary>
        /// <param name="commandLine"></param>
        /// <param name="inputOptions"></param>
        /// <param name="options"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<int> ExecuteAsync(string commandLine,
                               InputOptions inputOptions = default,
                               ModeBehavior options = default,
                               CancellationToken cancellationToken = default);
    }
}