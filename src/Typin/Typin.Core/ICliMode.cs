namespace Typin
{
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
        ValueTask Execute(ICliCommandExecutor executor);
    }
}
