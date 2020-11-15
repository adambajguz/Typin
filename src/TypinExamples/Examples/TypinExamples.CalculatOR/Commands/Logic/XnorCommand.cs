namespace TypinExamples.CalculatOR.Commands.Logic
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Typin;
    using Typin.Attributes;
    using Typin.Console;
    using TypinExamples.CalculatOR.Domain;
    using TypinExamples.CalculatOR.Utils;

    [Command("xnor", Description = "Performs a logical 'XNOR' operation on two or more numbers.")]
    public class XnorCommand : ICommand
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

        public XnorCommand(OperationEvaluatorService evaluator)
        {
            _evaluator = evaluator;
        }

        public async ValueTask ExecuteAsync(IConsole console)
        {
            await _evaluator.Eval(A, B, "XNOR", ShowSteps, Base, (x, y) => ~(x^ y));
        }
    }
}