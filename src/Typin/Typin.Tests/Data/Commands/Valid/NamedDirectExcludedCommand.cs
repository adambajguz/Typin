namespace Typin.Tests.Data.Commands.Valid
{
    using System.Threading.Tasks;
    using Typin;
    using Typin.Attributes;
    using Typin.Console;
    using Typin.Modes;

    [Command("named-direct-excluded-only", Description = "Named command description",
             ExcludedModes = new[] { typeof(DirectMode) })]
    public class NamedDirectExcludedCommand : ICommand
    {
        public const string ExpectedOutputText = nameof(NamedDirectExcludedCommand);

        public ValueTask ExecuteAsync(IConsole console)
        {
            console.Output.WriteLine(ExpectedOutputText);

            return default;
        }
    }
}