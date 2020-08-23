namespace Typin.Directives
{
    using System.Threading.Tasks;
    using Typin.Attributes;
    using Typin.Console;
    using Typin.Exceptions;

    [Directive("custom-throwable-with-message", Description = "Custom throwable directive with message.")]
    public sealed class CustomThrowableDirectiveWithMessage : IDirective
    {
        public const string ExpectedOutput = nameof(CustomThrowableDirectiveWithMessage);
        public const string ExpectedExceptionMessage = nameof(CustomThrowableDirectiveWithMessage) + "ExMessage";
        public const int ExpectedExitCode = 2;

        public bool ContinueExecution => true;

        public CustomThrowableDirectiveWithMessage()
        {

        }

        public ValueTask HandleAsync(IConsole console)
        {
            console.Output.Write(ExpectedOutput);

            throw new DirectiveException(ExpectedExceptionMessage, ExpectedExitCode);
        }
    }
}
