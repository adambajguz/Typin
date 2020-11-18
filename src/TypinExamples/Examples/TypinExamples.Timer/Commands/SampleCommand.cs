namespace TypinExamples.Timer.Commands
{
    using System.Threading.Tasks;
    using Typin;
    using Typin.Attributes;
    using Typin.Console;

    [Command]
    public class SampleCommand : ICommand
    {
        public async ValueTask ExecuteAsync(IConsole console)
        {
            await console.Output.WriteLineAsync(typeof(SampleCommand).AssemblyQualifiedName);
            await Task.Delay(100);
        }
    }
}