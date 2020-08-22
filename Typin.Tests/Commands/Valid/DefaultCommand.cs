namespace Typin.Tests.Commands.Valid
{
    using System.Threading.Tasks;
    using Typin;
    using Typin.Attributes;
    using Typin.Console;

    [Command(Description = "Default command description")]
    public class DefaultCommand : ICommand
    {
        public const string ExpectedOutputText = nameof(DefaultCommand);

        public ValueTask ExecuteAsync(IConsole console)
        {
            console.Output.WriteLine(ExpectedOutputText);

            return default;
        }
    }
}