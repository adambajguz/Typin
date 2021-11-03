namespace Typin.Tests.Data.CustomDirectives.Valid
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using PackSite.Library.Pipelining;
    using Typin;
    using Typin.Attributes;
    using Typin.Console;

    [Directive("custom-throwable", Description = "Custom throwable directive.")]
    public sealed class CustomThrowableDirective : IPipelinedDirective
    {
        public const string ExpectedOutput = nameof(CustomThrowableDirective);
        public const int ExpectedExitCode = 1;

        private readonly IConsole _console;

        public CustomThrowableDirective(IConsole console)
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

            throw new NullReferenceException();
        }
    }
}
