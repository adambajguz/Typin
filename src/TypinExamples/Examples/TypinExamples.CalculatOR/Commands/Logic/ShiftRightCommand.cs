namespace TypinExamples.CalculatOR.Commands.Logic
{
    using System.Numerics;
    using System.Threading.Tasks;
    using Typin;
    using Typin.Attributes;
    using Typin.Console;
    using TypinExamples.CalculatOR.Domain;

    [Command("shiftright", Description = "Logic right shift n bits")]
    public class ShiftRightCommand : ICommand
    {
        [CommandParameter(0)]
        public Number A { get; set; }

        [CommandParameter(1)]
        public int N { get; set; }

        public async ValueTask ExecuteAsync(IConsole console)
        {
                BigInteger score = A.Value >> N;
                await console.Output.WriteLineAsync(A.Value + " shift right " + N + " bits = " + score);
            
        }
    }
}