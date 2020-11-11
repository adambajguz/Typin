namespace Typin.Tests.Data.Commands.Valid
{
    using System.Threading.Tasks;
    using Typin;
    using Typin.Attributes;
    using Typin.Console;
    using Typin.Modes;

    [Command("named-interactive-only", Description = "Named command description",
             SupportedModes = new[] { typeof(InteractiveMode) })]
    public class NamedInteractiveOnlyCommand : ICommand
    {
        public const string ExpectedOutputText = nameof(NamedInteractiveOnlyCommand);

        public ValueTask ExecuteAsync(IConsole console)
        {
            console.Output.WriteLine(ExpectedOutputText);

            return default;
        }
    }
}