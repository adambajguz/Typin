namespace TypinExamples.CalculatOR.Commands.Logic
{
    using System.Collections.Generic;
    using System.Numerics;
    using System.Threading.Tasks;
    using Typin;
    using Typin.Attributes;
    using Typin.Console;
    using TypinExamples.CalculatOR.Domain;

    [Command("nand", Description = "The result of a logical operation 'nand' n consecutive numbers with the first number")]
    public class NandCommand : ICommand
    {
        [CommandParameter(0)]
        public Number A { get; set; }

        [CommandParameter(1)]
        public IEnumerable<Number> B { get; set; }

        public async ValueTask ExecuteAsync(IConsole console)
        {
            BigInteger counter = A.Value;
            foreach (var x in B)
            {
                BigInteger score = ~(counter & x.Value);
                await console.Output.WriteLineAsync(counter + " nand " + x.Value + " = " + score);
            }
        }
    }
}