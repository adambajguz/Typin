namespace Typin.Tests.Data.CustomDirectives.Valid
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using PackSite.Library.Pipelining;
    using Typin;
    using Typin.Attributes;
    using Typin.Console;

    [Directive("custom-throwable-with-message-and-show-help", Description = "Custom throwable directive with message and show help.")]
    public sealed class CustomThrowableDirectiveWithMessageAndShowHelp : IPipelinedDirective
    {
        public const string ExpectedOutput = nameof(CustomThrowableDirectiveWithMessageAndShowHelp);
        public const string ExpectedExceptionMessage = nameof(CustomThrowableDirectiveWithMessageAndShowHelp) + "ExMessage";
        public const int ExpectedExitCode = 2;

        private readonly IConsole _console;

        public CustomThrowableDirectiveWithMessageAndShowHelp(IConsole console)
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
