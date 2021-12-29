namespace Typin.Tests.Data.Valid.CustomDirectives
{
    using System.Threading;
    using System.Threading.Tasks;
    using PackSite.Library.Pipelining;
    using Typin;
    using Typin.Console;
    using Typin.Directives;
    using Typin.Directives.Attributes;

    [Directive("custom-interactive", Description = "Custom interactive only directive.")]
    public sealed class CustomInteractiveModeOnlyDirective : IDirective //TODO: add directive hadnler
    {
        public const string ExpectedOutput = nameof(CustomInteractiveModeOnlyDirective);

        private readonly IConsole _console;

        public CustomInteractiveModeOnlyDirective(IConsole console)
        {
            _console = console;
        }

        public async ValueTask ExecuteAsync(CliContext args, StepDelegate next, IInvokablePipeline<CliContext> invokablePipeline, CancellationToken cancellationToken = default)
        {
            _console.Output.Write(ExpectedOutput);

            await next();
        }
    }
}
