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

    [Directive("custom-throwable-with-message", Description = "Custom throwable directive with message.")]
    public sealed class CustomThrowableDirectiveWithMessage : IDirective
    {
        public const string ExpectedOutput = nameof(CustomThrowableDirectiveWithMessage);
        public const string ExpectedExceptionMessage = nameof(CustomThrowableDirectiveWithMessage) + "ExMessage";
        public const int ExpectedExitCode = 2;

        private readonly IConsole _console;

        public CustomThrowableDirectiveWithMessage(IConsole console)
        {
            _console = console;
        }

        public ValueTask ExecuteAsync(CliContext args, StepDelegate next, IInvokablePipeline<CliContext> invokablePipeline, CancellationToken cancellationToken = default)
        {
            _console.Output.Write(ExpectedOutput);

            throw new NullReferenceException(ExpectedExceptionMessage);
        }
    }
}
