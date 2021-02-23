namespace TypinExamples.Timer.Directives
{
    using System.Threading;
    using System.Threading.Tasks;
    using Typin;
    using Typin.Attributes;

    [Directive("print-perf", Description = "Enables performace printing before and after commmand execution.")]
    public class PrintPerformanceDirective : IDirective
    {
        public ValueTask OnInitializedAsync(CancellationToken cancellationToken)
        {
            return default;
        }
    }
}
