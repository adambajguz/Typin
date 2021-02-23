namespace Typin.Tests.Data.CustomDirectives.Valid
{
    using System.Threading;
    using System.Threading.Tasks;
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

        public ValueTask HandleAsync(ICliContext context, CommandPipelineHandlerDelegate next, CancellationToken _)
        {
            context.Console.Output.Write(ExpectedOutput);
            context.ExitCode ??= ExpectedExitCode;

            return default;
        }
    }
}
