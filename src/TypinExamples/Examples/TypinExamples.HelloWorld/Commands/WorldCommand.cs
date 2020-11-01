namespace TypinExamples.HelloWorld.Commands
{
    using System.Threading.Tasks;
    using Typin;
    using Typin.Attributes;
    using Typin.Console;

    [Command("world", Description = "Prints 'world' definition.")]
    public class WorldCommand : ICommand
    {
        public ValueTask ExecuteAsync(IConsole console)
        {
            console.Output.WriteLine("The world is the Earth and all life on it, including human civilisation.");

            return default;
        }
    }
}