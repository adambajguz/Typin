namespace Typin
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// CLI command executor.
    /// A scope is defined as a lifetime of a command execution pipeline that includes directives handling.
    /// </summary>
    public interface ICliCommandExecutor
    {
        /// <summary>
        /// Executes a command.
        /// </summary>
        Task<int> ExecuteCommandAsync(IEnumerable<string> commandLineArguments);

        /// <summary>
        /// Executes a command.
        /// </summary>
        Task<int> ExecuteCommandAsync(string commandLine);
    }
}