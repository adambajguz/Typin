namespace Typin.Tests.Data.CustomDirectives.Invalid
{
    using System.Threading.Tasks;
    using Typin.Console;

    public class NoAttributeDirective : IDirective
    {
        public bool ContinueExecution => true;

        public NoAttributeDirective()
        {

        }

        public ValueTask HandleAsync(IConsole console)
        {
            return default;
        }
    }
}
