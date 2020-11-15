namespace Typin.Tests.Data.CustomDirectives.Valid
{
    using System.Threading;
    using System.Threading.Tasks;
    using Typin;
    using Typin.Attributes;
    using Typin.Exceptions;

    [Directive("custom-throwable-with-message-and-show-help", Description = "Custom throwable directive with message and show help.")]
    public sealed class CustomThrowableDirectiveWithMessageAndShowHelp : IDirective
    {
        public const string ExpectedOutput = nameof(CustomThrowableDirectiveWithMessageAndShowHelp);
        public const string ExpectedExceptionMessage = nameof(CustomThrowableDirectiveWithMessageAndShowHelp) + "ExMessage";
        public const int ExpectedExitCode = 2;

        public ValueTask OnInitializedAsync(CancellationToken cancellationToken)
        {
            return default;
        }

        public ValueTask HandleAsync(ICliContext context, CommandPipelineHandlerDelegate next, CancellationToken _)
        {
            context.Console.Output.Write(ExpectedOutput);

            throw new DirectiveException(ExpectedExceptionMessage, ExpectedExitCode, true);
        }
    }
}
