namespace TypinExamples.CalculatOR.Commands.Arithmetic
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Numerics;
    using System.Threading.Tasks;
    using Typin;
    using Typin.Attributes;
    using Typin.Console;
    using TypinExamples.CalculatOR.Domain;

    [Command("add", Description = "The result of adding n consecutive numbers to the first number")]
    public class AddCommand : ICommand
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
                BigInteger sum = counter + x.Value;
                await console.Output.WriteLineAsync(counter + "+" + x.Value + "=" + sum );
            }
        }
    }
}