namespace TypinExamples.Timer.Commands
{
    using System.Threading.Tasks;
    using Typin;
    using Typin.Attributes;
    using Typin.Console;

    [Command(Description = "Default command that has a 100ms delay.")]
    public class DefaultCommand : ICommand
    {
        public async ValueTask ExecuteAsync(IConsole console)
        {
            await console.Output.WriteLineAsync(typeof(DefaultCommand).AssemblyQualifiedName);
            await Task.Delay(100);
        }
    }
}