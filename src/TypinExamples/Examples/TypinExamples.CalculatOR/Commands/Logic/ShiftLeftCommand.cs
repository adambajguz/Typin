namespace TypinExamples.CalculatOR.Commands.Logic
{
    using System.Threading.Tasks;
    using Typin;
    using Typin.Attributes;
    using Typin.Console;
    using TypinExamples.CalculatOR.Domain;

    [Command]
    public class ShiftLeftCommand : ICommand
    {
        [CommandParameter(0)]
        public Number A { get; set; }

        [CommandOption('n')]
        public uint N { get; set; }

        public ValueTask ExecuteAsync(IConsole console)
        {
            return default;
        }
    }
}