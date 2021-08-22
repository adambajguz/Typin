namespace Typin.Tests.Data.CustomDirectives.Valid
{
    using System.Threading;
    using System.Threading.Tasks;
    using PackSite.Library.Pipelining;
    using Typin;
    using Typin.Attributes;
    using Typin.Modes;

    [Directive("custom-interactive", Description = "Custom interactive only directive.", SupportedModes = new[] { typeof(InteractiveMode) })]
    public sealed class CustomInteractiveModeOnlyDirective : IPipelinedDirective
    {
        public const string ExpectedOutput = nameof(CustomInteractiveModeOnlyDirective);

        public ValueTask OnInitializedAsync(CancellationToken cancellationToken)
        {
            return default;
        }

        public async ValueTask ExecuteAsync(ICliContext args, StepDelegate next, IInvokablePipeline<ICliContext> invokablePipeline, CancellationToken cancellationToken = default)
        {
            args.Console.Output.Write(ExpectedOutput);

            await next();
        }
    }
}
