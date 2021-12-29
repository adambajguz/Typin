namespace Typin.Tests.Data.Valid.CustomDirectives
{
    using System.Threading;
    using System.Threading.Tasks;
    using PackSite.Library.Pipelining;
    using Typin;
    using Typin.Console;
    using Typin.Directives;
    using Typin.Directives.Attributes;

    [Directive("custom-stop", Description = "Custom stop directive.")]
    public sealed class CustomStopDirective : IDirective
    {
        public const string ExpectedOutput = nameof(CustomStopDirective);
        public const int ExpectedExitCode = 2;

        private readonly IConsole _console;

        public CustomStopDirective(IConsole console)
        {
            _console = console;
        }

        public ValueTask ExecuteAsync(CliContext args, StepDelegate next, IInvokablePipeline<CliContext> invokablePipeline, CancellationToken cancellationToken = default)
        {
            _console.Output.Write(ExpectedOutput);
            args.Output.ExitCode ??= ExpectedExitCode;

            return default;
        }
    }
}
