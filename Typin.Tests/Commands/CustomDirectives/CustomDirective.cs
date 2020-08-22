namespace Typin.Directives
{
    using System.Threading.Tasks;
    using Typin.Attributes;
    using Typin.Console;

    [Directive("custom", Description = "Custom directive.")]
    public sealed class CustomDirective : IDirective
    {
        public const string ExpectedOutput = nameof(CustomDirective);

        public bool ContinueExecution => true;

        public CustomDirective()
        {

        }

        public ValueTask HandleAsync(IConsole console)
        {
            console.Output.Write(ExpectedOutput);

            return default;
        }
    }
}
