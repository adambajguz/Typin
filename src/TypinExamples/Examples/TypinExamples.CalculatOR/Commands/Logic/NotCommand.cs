namespace TypinExamples.CalculatOR.Commands.Logic
{
    using System.Collections.Generic;
    using System.Numerics;
    using System.Threading.Tasks;
    using Typin;
    using Typin.Attributes;
    using Typin.Console;
    using TypinExamples.CalculatOR.Domain;

    [Command("not", Description = "The result of a logical operation 'not' on a number")]
    public class NotCommand : ICommand
    {
        [CommandParameter(0)]
        public Number A { get; set; }

        public async ValueTask ExecuteAsync(IConsole console)
        {
            BigInteger score = ~A.Value;
            await console.Output.WriteLineAsync( "Not " + A.Value + " = " + score);

        }
    }
}