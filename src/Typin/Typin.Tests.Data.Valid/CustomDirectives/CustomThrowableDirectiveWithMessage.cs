namespace Typin.Tests.Data.CustomDirectives.Valid
{
    using System.Threading;
    using System.Threading.Tasks;
    using PackSite.Library.Pipelining;
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

        public async ValueTask ExecuteAsync(ICliContext args, StepDelegate next, IInvokablePipeline<ICliContext> invokablePipeline, CancellationToken cancellationToken = default)
        {
            args.Console.Output.Write(ExpectedOutput);

            throw new DirectiveException(ExpectedExceptionMessage, ExpectedExitCode);
        }
    }
}
