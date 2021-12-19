namespace InteractiveModeExample.Directives
{
    using System.Diagnostics.CodeAnalysis;
    using System.Threading;
    using System.Threading.Tasks;
    using PackSite.Library.Pipelining;
    using Typin;
    using Typin.Attributes;
    using Typin.Console;
    using Typin.Modes.Interactive;

    [Directive("custom-interactive", Description = "Custom interactive only directive.",
               SupportedModes = new[] { typeof(InteractiveMode) })]
    public sealed class CustomInteractiveModeOnlyDirective : IPipelinedDirective
    {
        public const string ExpectedOutput = nameof(CustomInteractiveModeOnlyDirective);

        private readonly IConsole _console;

        public CustomInteractiveModeOnlyDirective(IConsole console)
        {
            _console = console;
        }

        public ValueTask InitializeAsync(CancellationToken cancellationToken)
        {
            _console.Output.Write(nameof(CustomInteractiveModeOnlyDirective) + " INIT");

            return default;
        }

        public async ValueTask ExecuteAsync(CliContext args, StepDelegate next, IInvokablePipeline<CliContext> invokablePipeline, CancellationToken cancellationToken = default)
        {
            _console.Output.Write(nameof(CustomInteractiveModeOnlyDirective) + " >>>");

            await next();

            _console.Output.Write(nameof(CustomInteractiveModeOnlyDirective) + " <<<");
        }
    }
}
