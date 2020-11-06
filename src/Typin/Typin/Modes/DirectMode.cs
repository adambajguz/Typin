namespace Typin.Modes
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Typin.Internal;

    /// <summary>
    /// Direct CLI mode.
    /// </summary>
    public class DirectMode : ICliMode
    {
        /// <summary>
        /// Initializes an instance of <see cref="DirectMode"/>.
        /// </summary>
        public DirectMode()
        {

        }

        /// <inheritdoc/>
        public async ValueTask<int> Execute(IReadOnlyList<string> commandLineArguments, ICliCommandExecutor executor)
        {
            return await executor.ExecuteCommand(commandLineArguments);
        }
    }
}
