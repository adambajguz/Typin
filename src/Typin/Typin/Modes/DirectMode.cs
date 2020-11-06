namespace Typin.Modes
{
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
        public ValueTask Execute(ICliCommandExecutor executor)
        {
            throw new System.NotImplementedException();
        }
    }
}
