namespace Typin.Tests.Data.CustomDirectives.Valid
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using PackSite.Library.Pipelining;
    using Typin;
    using Typin.Attributes;
    using Typin.Console;

    [Directive("custom-throwable-with-inner-exception", Description = "Custom throwable directive with message.")]
    public sealed class CustomThrowableDirectiveWithInnerException : IPipelinedDirective
    {
        public const string ExpectedOutput = nameof(CustomThrowableDirectiveWithInnerException);
        public const string ExpectedExceptionMessage = nameof(CustomThrowableDirectiveWithInnerException) + "ExMessage";
        public const int ExpectedExitCode = 2;

        private readonly IConsole _console;

        public CustomThrowableDirectiveWithInnerException(IConsole console)
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

            throw new NullReferenceException(ExpectedExceptionMessage);
        }
    }
}
