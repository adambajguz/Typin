namespace Typin.Tests.Data.CustomDirectives.Invalid
{
    using System.Threading.Tasks;
    using Typin.Attributes;
    using Typin.Console;

    [Directive("invalid-no-interface", Description = "Abstract directive.")]
    public class NoInterafaceDirective
    {
        public bool ContinueExecution => true;

        public NoInterafaceDirective()
        {

        }

        public ValueTask HandleAsync(IConsole console)
        {
            return default;
        }
    }
}
