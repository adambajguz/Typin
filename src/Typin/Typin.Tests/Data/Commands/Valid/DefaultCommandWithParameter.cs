namespace Typin.Tests.Data.Commands.Valid
{
    using System.Threading.Tasks;
    using Typin;
    using Typin.Attributes;
    using Typin.Console;

    [Command(Description = "Default command with parameter description")]
    public class DefaultCommandWithParameter : ICommand
    {
        public const string ExpectedOutputText = nameof(DefaultCommandWithParameter);

        [CommandParameter(0)]
        public string? ParamA { get; set; }

        public ValueTask ExecuteAsync(IConsole console)
        {
            console.Output.WriteLine(ExpectedOutputText);
            console.Output.WriteLine(ParamA);

            return default;
        }
    }
}