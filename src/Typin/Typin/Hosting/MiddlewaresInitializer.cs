namespace Typin.Hosting
{
    using System.Threading;
    using System.Threading.Tasks;
    using PackSite.Library.Pipelining;
    using Typin.Help;
    using Typin.Pipeline;

    /// <summary>
    /// Initializes middlewares.
    /// </summary>
    internal sealed class MiddlewaresInitializer : IPipelineInitializer
    {
        /// <inheritdoc/>
        public ValueTask RegisterAsync(IPipelineCollection pipelines, CancellationToken cancellationToken)
        {
            _ = PipelineBuilder.Create<CliContext>()
                 .Lifetime(InvokablePipelineLifetime.Scoped)
                 .AddStep<TypinExceptionsHandler>()
                 .AddStep<ResolveInput>()
                 .AddStep<InitializeBinder>()
                 .AddStep<ResolveCommand>()
                 .AddStep<InitializeDirectives>()
                 .AddStep<PipelinedDirectivesHandler>()
                 .AddStep<HelpHandler>()
                 .AddStep<BindInput>()
                 // user
                 .AddStep<ExecuteCommand>()
                 .Build().TryAddTo(pipelines);

            return default;
        }
    }
}
