namespace TypinExamples.Timer.Directives
{
    using System.Threading;
    using System.Threading.Tasks;
    using Typin;
    using Typin.Attributes;

    [Directive("no-log", Description = "Disables performance logging for current command.")]
    public class NoLoggingDirective : IDirective
    {
        public ValueTask OnInitializedAsync(CancellationToken cancellationToken)
        {
            return default;
        }
    }
}
