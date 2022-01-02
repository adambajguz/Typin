namespace TypinExamples.HelloWorld.Commands
{
    using System.Threading.Tasks;
    using Typin;
    using Typin.Attributes;
    using Typin.Console;
    using Typin.Exceptions;

    [Command("world", Description = "Prints 'world' definition, then throws CommandException to print help.")]
    public class WorldCommand : ICommand
    {
        public ValueTask ExecuteAsync(IConsole console)
        {
            console.Output.WriteLine("The world is the Earth and all life on it, including human civilisation.");

            throw new CommandException(string.Empty, showHelp: true);
        }
    }
}