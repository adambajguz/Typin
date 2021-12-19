namespace InteractiveModeExample.Directives
{
    using System.Diagnostics.CodeAnalysis;
    using System.Threading;
    using System.Threading.Tasks;
    using Typin;
    using Typin.Attributes;
    using Typin.Modes;

    [Directive("custom-interactive", Description = "Custom interactive only directive.",
               SupportedModes = new[] { typeof(InteractiveMode) })]
    public sealed class CustomInteractiveModeOnlyDirective : IDirective
    {
        public const string ExpectedOutput = nameof(CustomInteractiveModeOnlyDirective);

        public ValueTask OnInitializedAsync(CancellationToken cancellationToken)
        {
            return default;
        }

        [SuppressMessage("Performance", "CA1822:Mark members as static")]
        public async ValueTask HandleAsync(ICliContext context, CommandPipelineHandlerDelegate next, CancellationToken cancellationToken)
        {
            context.Console.Output.Write(ExpectedOutput);

            await next();
        }
    }
}
