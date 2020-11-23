namespace TypinExamples.CalculatOR.Commands.Logic
{
    using System.Numerics;
    using System.Threading.Tasks;
    using Typin;
    using Typin.Attributes;
    using Typin.Console;
    using TypinExamples.CalculatOR.Domain;
    using TypinExamples.CalculatOR.Services;

    [Command("rsh", Description = "Performs a logic right shift by 'n' bits.")]
    public class ShiftRightCommand : ICommand
    {
        [CommandParameter(0)]
        public Number A { get; set; }

        [CommandOption('n', IsRequired = true)]
        public int N { get; set; }

        [CommandOption("steps", 's', Description = "Whether to show steps.")]
        public bool ShowSteps { get; set; }

        [CommandOption("base", 'b', Description = "Base that will be used for displaying all numbers. Default means auto.")]
        public NumberBase? Base { get; set; } = null;

        private readonly OperationEvaluatorService _evaluator;

        public ShiftRightCommand(OperationEvaluatorService evaluator)
        {
            _evaluator = evaluator;
        }

        public async ValueTask ExecuteAsync(IConsole console)
        {
            await _evaluator.Eval(A, new[] { new Number(new BigInteger(N), NumberBase.DEC) }, ">>", ShowSteps, Base, (x, y) => x >> N);
        }
    }
}