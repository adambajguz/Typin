namespace TypinExamples.Validation.Commands
{
    using System.Threading.Tasks;
    using Typin;
    using Typin.Attributes;
    using Typin.Console;

    [Command("person", Description = "Person validation example.")]
    public class PersonCommand : ICommand
    {
        [CommandParameter(0)]
        public string? Name { get; set; }

        [CommandParameter(1)]
        public int Age { get; set; }

        public async ValueTask ExecuteAsync(IConsole console)
        {
            await console.Output.WriteLineAsync($"{Name} is {Age} years old.");
        }
    }
}