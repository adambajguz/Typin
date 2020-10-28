namespace TypinExamples.HelloWorld.Commands
{
    using System.Threading.Tasks;
    using Typin;
    using Typin.Attributes;
    using Typin.Console;

    [Command]
    public class SimpleCommand : ICommand
    {
        public async ValueTask ExecuteAsync(IConsole console)
        {
            await console.Output.WriteLineAsync("Hello World!");
        }
    }
}