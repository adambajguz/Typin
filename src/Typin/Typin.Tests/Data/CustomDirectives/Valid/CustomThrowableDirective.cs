namespace Typin.Tests.Data.CustomDirectives.Valid
{
    using System.Threading;
    using System.Threading.Tasks;
    using Typin;
    using Typin.Attributes;
    using Typin.Exceptions;

    [Directive("custom-throwable", Description = "Custom throwable directive.")]
    public sealed class CustomThrowableDirective : IDirective
    {
        public const string ExpectedOutput = nameof(CustomThrowableDirective);
        public const int ExpectedExitCode = 1;

        public ValueTask OnInitializedAsync(CancellationToken cancellationToken)
        {
            return default;
        }

        public ValueTask HandleAsync(ICliContext context, CommandPipelineHandlerDelegate next, CancellationToken _)
        {
            context.Console.Output.Write(ExpectedOutput);

            throw new DirectiveException();
        }
    }
}
