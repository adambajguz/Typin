namespace Typin.Directives
{
    using System;
    using System.Threading.Tasks;
    using Typin.Attributes;
    using Typin.Console;
    using Typin.Exceptions;

    [Directive("custom-throwable-with-inner-exception", Description = "Custom throwable directive with message.")]
    public sealed class CustomThrowableDirectiveWithInnerException : IDirective
    {
        public const string ExpectedOutput = nameof(CustomThrowableDirectiveWithInnerException);
        public const string ExpectedExceptionMessage = nameof(CustomThrowableDirectiveWithInnerException) + "ExMessage";
        public const int ExpectedExitCode = 2;

        public bool ContinueExecution => true;

        public CustomThrowableDirectiveWithInnerException()
        {

        }

        public ValueTask HandleAsync(IConsole console)
        {
            console.Output.Write(ExpectedOutput);

            throw new DirectiveException(ExpectedExceptionMessage, new NullReferenceException(), ExpectedExitCode);
        }
    }
}
