namespace Typin.Tests.Commands.Valid
{
    using System.Threading.Tasks;
    using Typin;
    using Typin.Attributes;
    using Typin.Console;

    [Command("named sub", Description = "Named sub command description")]
    public class NamedSubCommand : ICommand
    {
        public const string ExpectedOutputText = nameof(NamedSubCommand);

        public ValueTask ExecuteAsync(IConsole console)
        {
            console.Output.WriteLine(ExpectedOutputText);

            return default;
        }
    }
}