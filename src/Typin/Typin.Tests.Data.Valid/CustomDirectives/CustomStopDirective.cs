namespace Typin.Tests.Data.CustomDirectives.Valid
{
    using System.Threading;
    using System.Threading.Tasks;
    using PackSite.Library.Pipelining;
    using Typin;
    using Typin.Attributes;
    using Typin.Console;

    [Directive("custom-stop", Description = "Custom stop directive.")]
    public sealed class CustomStopDirective : IPipelinedDirective
    {
        public const string ExpectedOutput = nameof(CustomStopDirective);
        public const int ExpectedExitCode = 2;

        private readonly IConsole _console;

        public CustomStopDirective(IConsole console)
        {
            _console = console;
        }

        public ValueTask InitializeAsync(CancellationToken cancellationToken)
        {
            return default;
        }

        public ValueTask ExecuteAsync(CliContext args, StepDelegate next, IInvokablePipeline<CliContext> invokablePipeline, CancellationToken cancellationToken = default)
        {
            _console.Output.Write(ExpectedOutput);
            args.Output.ExitCode ??= ExpectedExitCode;

            return default;
        }
    }
}
