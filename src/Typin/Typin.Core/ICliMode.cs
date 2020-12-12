namespace Typin
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// CLI mode definition.
    /// </summary>
    public interface ICliMode
    {
        /// <summary>
        /// Executes CLI mode.
        /// </summary>
        ValueTask<int> ExecuteAsync(IEnumerable<string> commandLineArguments, ICliCommandExecutor executor);
    }
}
