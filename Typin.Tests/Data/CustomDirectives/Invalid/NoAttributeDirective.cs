namespace Typin.Tests.Data.CustomDirectives.Invalid
{
    using System.Threading.Tasks;
    using Typin.Attributes;
    using Typin.Console;
    using Typin.Tests.Data.CustomDirectives.Valid;

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
