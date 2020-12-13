namespace TypinExamples.Validation.Commands
{
    using System.Threading.Tasks;
    using Typin;
    using Typin.Attributes;
    using Typin.Console;

    [Command("person", Description = "Person validation example.")]
    public class PersonCommand : ICommand
    {
        [CommandParameter(0, Description = "Person name.")]
        public string? Name { get; set; }

        [CommandParameter(1, Description = "Person age.")]
        public int Age { get; set; }

        public async ValueTask ExecuteAsync(IConsole console)
        {
            await console.Output.WriteLineAsync($"{Name} is {Age} years old.");
        }
    }
}