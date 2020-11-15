namespace TypinExamples.CalculatOR.Commands.Arithmetic
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Typin;
    using Typin.Attributes;
    using Typin.Console;
    using TypinExamples.CalculatOR.Domain;
    using TypinExamples.CalculatOR.Utils;

    [Command("multiply", Description = "Multiplies two or more numbers.")]
    public class MultiplyCommand : ICommand
    {
        [CommandParameter(0)]
        public Number A { get; set; }

        [CommandParameter(1)]
        public IEnumerable<Number> B { get; set; } = default!;

        [CommandOption("steps", 's', Description = "Whether to show steps.")]
        public bool ShowSteps { get; set; }

        [CommandOption("base", 'b', Description = "Base that will be used for displaying all numbers. Default means auto.")]
        public NumberBase? Base { get; set; } = null;

        private readonly OperationEvaluatorService _evaluator;

        public MultiplyCommand(OperationEvaluatorService evaluator)
        {
            _evaluator = evaluator;
        }

        public async ValueTask ExecuteAsync(IConsole console)
        {
            await _evaluator.Eval(A, B, "*", ShowSteps, Base, (x, y) => x * y);
        }
    }
}