namespace TypinExamples.InteractiveQuery.Commands
{
    using System.Threading.Tasks;
    using Typin;
    using Typin.Attributes;
    using Typin.Console;

    [Command]
    public class SampleCommand : ICommand
    {
        [CommandParameter(0)]
        public int? ParamB { get; set; }

        [CommandOption("str", 's')]
        public string? StrOption { get; set; }

        [CommandOption("int", 'i')]
        public int IntOption { get; set; }

        [CommandOption("bool", 'b')]
        public bool BoolOption { get; set; }

        [CommandOption('v')]
        public bool VOption { get; set; }

        [CommandOption('x')]
        public bool XOption { get; set; }

        public async ValueTask ExecuteAsync(IConsole console)
        {
            await console.Output.WriteLineAsync(typeof(SampleCommand).AssemblyQualifiedName);
        }
    }
}