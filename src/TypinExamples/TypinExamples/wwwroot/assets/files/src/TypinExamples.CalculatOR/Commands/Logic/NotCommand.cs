namespace TypinExamples.CalculatOR.Commands.Logic
{
    using System.Threading.Tasks;
    using Typin;
    using Typin.Attributes;
    using Typin.Console;
    using TypinExamples.CalculatOR.Domain;
    using TypinExamples.CalculatOR.Services;

    [Command("not", Description = "Performs a logical 'NOT' operation on a number.")]
    public class NotCommand : ICommand
    {
        [CommandParameter(0)]
        public Number A { get; set; }

        [CommandOption("steps", 's', Description = "Whether to show steps.")]
        public bool ShowSteps { get; set; }

        [CommandOption("base", 'b', Description = "Base that will be used for displaying all numbers. Default means auto.")]
        public NumberBase? Base { get; set; } = null;

        private readonly OperationEvaluatorService _evaluator;

        public NotCommand(OperationEvaluatorService evaluator)
        {
            _evaluator = evaluator;
        }

        public async ValueTask ExecuteAsync(IConsole console)
        {
            await _evaluator.Eval(A, new[] { A }, "NOT", ShowSteps, Base, (x, y) => ~(x));
        }
    }
}