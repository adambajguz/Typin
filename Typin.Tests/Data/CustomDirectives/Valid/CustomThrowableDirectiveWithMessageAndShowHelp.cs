namespace Typin.Tests.Data.CustomDirectives.Valid
{
    using System.Threading.Tasks;
    using Typin;
    using Typin.Attributes;
    using Typin.Console;
    using Typin.Exceptions;

    [Directive("custom-throwable-with-message-and-show-help", Description = "Custom throwable directive with message and show help.")]
    public sealed class CustomThrowableDirectiveWithMessageAndShowHelp : IDirective
    {
        public const string ExpectedOutput = nameof(CustomThrowableDirectiveWithMessageAndShowHelp);
        public const string ExpectedExceptionMessage = nameof(CustomThrowableDirectiveWithMessageAndShowHelp) + "ExMessage";
        public const int ExpectedExitCode = 2;

        public bool ContinueExecution => true;

        public CustomThrowableDirectiveWithMessageAndShowHelp()
        {

        }

        public ValueTask HandleAsync(IConsole console)
        {
            console.Output.Write(ExpectedOutput);

            throw new DirectiveException(ExpectedExceptionMessage, ExpectedExitCode, true);
        }
    }
}
