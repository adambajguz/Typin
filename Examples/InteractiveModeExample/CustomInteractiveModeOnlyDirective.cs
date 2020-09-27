namespace InteractiveModeExample
{
    using System.Threading.Tasks;
    using Typin;
    using Typin.Attributes;
    using Typin.Console;

    [Directive("custom-interactive", Description = "Custom interactive only directive.", InteractiveModeOnly = true)]
    public sealed class CustomInteractiveModeOnlyDirective : IDirective
    {
        public const string ExpectedOutput = nameof(CustomInteractiveModeOnlyDirective);

        public bool ContinueExecution => true;

        public CustomInteractiveModeOnlyDirective()
        {

        }

        public ValueTask HandleAsync(IConsole console)
        {
            console.Output.WriteLine(ExpectedOutput);

            return default;
        }
    }
}
