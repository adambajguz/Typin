namespace Typin.Tests.Data.CustomDirectives.Valid
{
    using System.Threading;
    using System.Threading.Tasks;
    using Typin;
    using Typin.Attributes;
    using Typin.Exceptions;

    [Directive("custom-throwable-with-message", Description = "Custom throwable directive with message.")]
    public sealed class CustomThrowableDirectiveWithMessage : IPipelinedDirective
    {
        public const string ExpectedOutput = nameof(CustomThrowableDirectiveWithMessage);
        public const string ExpectedExceptionMessage = nameof(CustomThrowableDirectiveWithMessage) + "ExMessage";
        public const int ExpectedExitCode = 2;

        public ValueTask OnInitializedAsync(CancellationToken cancellationToken)
        {
            return default;
        }

        public ValueTask HandleAsync(ICliContext context, CommandPipelineHandlerDelegate next, CancellationToken _)
        {
            context.Console.Output.Write(ExpectedOutput);

            throw new DirectiveException(ExpectedExceptionMessage, ExpectedExitCode);
        }
    }
}
