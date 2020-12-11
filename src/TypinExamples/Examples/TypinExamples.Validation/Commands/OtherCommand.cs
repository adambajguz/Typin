namespace TypinExamples.Validation.Commands
{
    using System.Threading.Tasks;
    using Typin;
    using Typin.Attributes;
    using Typin.Console;

    [Command("other")]
    public class OtherCommand : ICommand
    {
        [CommandParameter(0)]
        public string? Name { get; set; }

        public async ValueTask ExecuteAsync(IConsole console)
        {
            await console.Output.WriteLineAsync(typeof(SampleCommand).AssemblyQualifiedName);
        }
    }
}