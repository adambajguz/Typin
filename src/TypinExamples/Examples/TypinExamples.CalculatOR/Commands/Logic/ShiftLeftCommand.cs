namespace TypinExamples.CalculatOR.Commands.Logic
{
    using System.Numerics;
    using System.Threading.Tasks;
    using Typin;
    using Typin.Attributes;
    using Typin.Console;
    using TypinExamples.CalculatOR.Domain;

    [Command("shiftleft", Description = "Logic left shift n bits")]
    public class ShiftLeftCommand : ICommand
    {
        [CommandParameter(0)]
        public Number A { get; set; }

        [CommandParameter(1)]
        public int N { get; set; }

        public async ValueTask ExecuteAsync(IConsole console)
        {
            BigInteger score = A.Value << N;
            await console.Output.WriteLineAsync(A.Value + " shift left " + N + " bits = " + score);

        }
    }
}