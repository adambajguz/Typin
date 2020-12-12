namespace Typin
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// CLI command executor.
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