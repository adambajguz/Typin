using System.Threading.Tasks;
using Typin.Internal;

namespace Typin.Modes
{
    /// <summary>
    /// Direct CLI mode.
    /// </summary>
    public class DirectMode : ICliMode
    {
        public DirectMode()
        {

        }

        public ValueTask Execute(ICliCommandExecutor executor)
        {
            throw new System.NotImplementedException();
        }
    }
}
