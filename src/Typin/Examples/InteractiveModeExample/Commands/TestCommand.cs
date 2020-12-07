namespace Typin.InteractiveModeDemo.Commands
{
    using System.Threading.Tasks;
    using Typin.Attributes;
    using Typin.Console;

    [Command("test", Description = "Test command.")]
    public class TestCommand : ICommand
    {
        [CommandOption("xe", 'a')]
        public string Author { get; set; } = string.Empty;

        [CommandOption('x')]
        public string AuthorX { get; set; } = string.Empty;

        [CommandOption("char", 'c')]
        public char Ch { get; set; }

        public ValueTask ExecuteAsync(IConsole console)
        {
            console.Output.WriteLine($"'{Author}' '{AuthorX}' '{Ch}'");

            return default;
        }
    }
}
