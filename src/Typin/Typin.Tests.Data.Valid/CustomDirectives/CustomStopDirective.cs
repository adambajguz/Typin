namespace Typin.Tests.Data.CustomDirectives.Valid
{
    using System.Threading;
    using System.Threading.Tasks;
    using PackSite.Library.Pipelining;
    using Typin;
    using Typin.Attributes;

    [Directive("custom-stop", Description = "Custom stop directive.")]
    public sealed class CustomStopDirective : IPipelinedDirective
    {
        public const string ExpectedOutput = nameof(CustomStopDirective);
        public const int ExpectedExitCode = 2;

        public ValueTask OnInitializedAsync(CancellationToken cancellationToken)
        {
            return default;
        }

        public ValueTask ExecuteAsync(ICliContext args, StepDelegate next, IInvokablePipeline<ICliContext> invokablePipeline, CancellationToken cancellationToken = default)
        {
            args.Console.Output.Write(ExpectedOutput);
            args.ExitCode ??= ExpectedExitCode;

            return default;
        }
    }
}
