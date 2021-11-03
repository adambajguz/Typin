namespace Typin.Tests.Data.CustomDirectives.Valid
{
    using System.Threading;
    using System.Threading.Tasks;
    using PackSite.Library.Pipelining;
    using Typin;
    using Typin.Attributes;
    using Typin.Console;

    [Directive("custom", Description = "Custom directive.")]
    public sealed class CustomDirective : IPipelinedDirective
    {
        public const string ExpectedOutput = nameof(CustomDirective);

        private readonly IConsole _console;

        public CustomDirective(IConsole console)
        {
            _console = console;
        }

        public ValueTask InitializeAsync(CancellationToken cancellationToken)
        {
            return default;
        }

        public async ValueTask ExecuteAsync(CliContext args, StepDelegate next, IInvokablePipeline<CliContext> invokablePipeline, CancellationToken cancellationToken = default)
        {
            _console.Output.Write(ExpectedOutput);

            await next();
        }
    }
}
