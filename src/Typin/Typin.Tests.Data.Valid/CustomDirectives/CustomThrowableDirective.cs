namespace Typin.Tests.Data.Valid.CustomDirectives
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using PackSite.Library.Pipelining;
    using Typin;
    using Typin.Attributes;
    using Typin.Console;
    using Typin.Directives;

    [Directive("custom-throwable", Description = "Custom throwable directive.")]
    public sealed class CustomThrowableDirective : IDirective
    {
        public const string ExpectedOutput = nameof(CustomThrowableDirective);
        public const int ExpectedExitCode = 1;

        private readonly IConsole _console;

        public CustomThrowableDirective(IConsole console)
        {
            _console = console;
        }

        public ValueTask ExecuteAsync(CliContext args, StepDelegate next, IInvokablePipeline<CliContext> invokablePipeline, CancellationToken cancellationToken = default)
        {
            _console.Output.Write(ExpectedOutput);

            throw new NullReferenceException();
        }
    }
}
