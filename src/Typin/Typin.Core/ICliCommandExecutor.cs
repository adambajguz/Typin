namespace Typin.Internal
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
        Task<int> ExecuteCommand(IEnumerable<string> commandLineArguments);


        /// <summary>
        /// Executes a command.
        /// </summary>
        Task<int> ExecuteCommand(IReadOnlyList<string> commandLineArguments);

        /// <summary>
        /// Executes a command.
        /// </summary>
        Task<int> ExecuteCommand(string commandLine);
    }
}