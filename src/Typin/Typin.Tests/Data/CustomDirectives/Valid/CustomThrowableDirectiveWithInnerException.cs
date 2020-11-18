namespace Typin.Tests.Data.CustomDirectives.Valid
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Typin;
    using Typin.Attributes;
    using Typin.Exceptions;

    [Directive("custom-throwable-with-inner-exception", Description = "Custom throwable directive with message.")]
    public sealed class CustomThrowableDirectiveWithInnerException : IPipelinedDirective
    {
        public const string ExpectedOutput = nameof(CustomThrowableDirectiveWithInnerException);
        public const string ExpectedExceptionMessage = nameof(CustomThrowableDirectiveWithInnerException) + "ExMessage";
        public const int ExpectedExitCode = 2;

        public ValueTask OnInitializedAsync(CancellationToken cancellationToken)
        {
            return default;
        }

        public ValueTask HandleAsync(ICliContext context, CommandPipelineHandlerDelegate next, CancellationToken _)
        {
            context.Console.Output.Write(ExpectedOutput);

            throw new DirectiveException(ExpectedExceptionMessage, new NullReferenceException(), ExpectedExitCode);
        }
    }
}
