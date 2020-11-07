namespace Typin
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Typin.Internal;

    /// <summary>
    /// CLI mode definition.
    /// </summary>
    public interface ICliMode
    {
        /// <summary>
        /// Executes CLI mode.
        /// </summary>
        ValueTask<int> Execute(IReadOnlyList<string> commandLineArguments, ICliCommandExecutor executor);
    }
}
