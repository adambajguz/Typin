namespace Typin.Tests.Data.CustomDirectives.Invalid
{
    using System.Threading.Tasks;
    using Typin;
    using Typin.Attributes;
    using Typin.Console;
    using Typin.Tests.Data.CustomDirectives.Valid;

    [Directive("preview", Description = "Duplicate directive.")]
    public class DuplicatedDirective : IDirective
    {
        public bool ContinueExecution => true;

        public DuplicatedDirective()
        {

        }

        public ValueTask HandleAsync(IConsole console)
        {
            return default;
        }
    }
}
