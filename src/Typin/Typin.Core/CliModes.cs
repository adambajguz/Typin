namespace Typin
{
    using System;
    using System.Threading.Tasks;
    using Typin.Internal;

    /// <summary>
    /// CLI modes.
    /// </summary>
    [Flags]
    public enum CliModes
    {
        /// <summary>
        /// Direct CLI tool mode.
        /// </summary>
        Direct = 1,

        /// <summary>
        /// Interactive CLI mode.
        /// </summary>
        Interactive = 2,

        /// <summary>
        /// Batch CLI mode.
        /// </summary>
        Batch = 4
    }

    /// <summary>
    /// CLI mode definition.
    /// </summary>
    public interface ICliMode
    {
        /// <summary>
        /// Executes CLI mode.
        /// </summary>
        ValueTask Execute(ICliCommandExecutor executor);
    }
}
