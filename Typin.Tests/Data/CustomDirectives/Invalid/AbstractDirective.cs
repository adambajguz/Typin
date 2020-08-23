namespace Typin.Tests.Data.CustomDirectives.Invalid
{
    using System.Threading.Tasks;
    using Typin;
    using Typin.Attributes;
    using Typin.Console;
    using Typin.Tests.Data.CustomDirectives.Valid;

    [Directive("invalid-abstract", Description = "Abstract directive.")]
    public abstract class AbstractDirective : IDirective
    {
        public bool ContinueExecution => true;

        public AbstractDirective()
        {

        }

        public ValueTask HandleAsync(IConsole console)
        {
            return default;
        }
    }
}
