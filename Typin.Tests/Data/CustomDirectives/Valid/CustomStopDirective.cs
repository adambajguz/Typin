namespace Typin.Tests.Data.CustomDirectives.Valid
{
    using System.Threading.Tasks;
    using Typin;
    using Typin.Attributes;
    using Typin.Console;

    [Directive("custom-stop", Description = "Custom stop directive.")]
    public sealed class CustomStopDirective : IDirective
    {
        public const string ExpectedOutput = nameof(CustomStopDirective);

        public bool ContinueExecution => false;

        public CustomStopDirective()
        {

        }

        public ValueTask HandleAsync(IConsole console)
        {
            console.Output.Write(ExpectedOutput);

            return default;
        }
    }
}
