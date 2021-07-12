namespace Typin.Tests.Data.CustomDirectives.Valid
{
    using System.Threading;
    using System.Threading.Tasks;
    using Typin;
    using Typin.Attributes;

    [Directive("custom", Description = "Custom directive.")]
    public sealed class CustomDirective : IPipelinedDirective
    {
        public const string ExpectedOutput = nameof(CustomDirective);

        public ValueTask OnInitializedAsync(CancellationToken cancellationToken)
        {
            return default;
        }

        public async ValueTask HandleAsync(ICliContext context, CommandPipelineHandlerDelegate next, CancellationToken _)
        {
            context.Console.Output.Write(ExpectedOutput);

            await next();
        }
    }
}
