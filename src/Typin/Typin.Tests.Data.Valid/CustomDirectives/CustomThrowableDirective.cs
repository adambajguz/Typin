namespace Typin.Tests.Data.CustomDirectives.Valid
{
    using System.Threading;
    using System.Threading.Tasks;
    using PackSite.Library.Pipelining;
    using Typin;
    using Typin.Attributes;
    using Typin.Exceptions;

    [Directive("custom-throwable", Description = "Custom throwable directive.")]
    public sealed class CustomThrowableDirective : IPipelinedDirective
    {
        public const string ExpectedOutput = nameof(CustomThrowableDirective);
        public const int ExpectedExitCode = 1;

        public ValueTask OnInitializedAsync(CancellationToken cancellationToken)
        {
            return default;
        }

        public async ValueTask ExecuteAsync(ICliContext args, StepDelegate next, IInvokablePipeline<ICliContext> invokablePipeline, CancellationToken cancellationToken = default)
        {
            args.Console.Output.Write(ExpectedOutput);

            throw new DirectiveException();
        }
    }
}
