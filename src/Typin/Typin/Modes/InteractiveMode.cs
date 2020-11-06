namespace Typin.Modes
{
    using System.Threading.Tasks;
    using Typin.Internal;

    /// <summary>
    /// Interactive CLI mode.
    /// </summary>
    public class InteractiveMode : ICliMode
    {
        /// <summary>
        /// Initializes an instance of <see cref="DirectMode"/>.
        /// </summary>
        public InteractiveMode()
        {

        }

        /// <inheritdoc/>
        public ValueTask Execute(ICliCommandExecutor executor)
        {
            throw new System.NotImplementedException();
        }
    }
}
