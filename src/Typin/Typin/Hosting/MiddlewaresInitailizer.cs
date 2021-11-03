namespace Typin.Hosting
{
    using System.Threading;
    using System.Threading.Tasks;
    using PackSite.Library.Pipelining;
    using Typin.Pipeline;

    /// <summary>
    /// Initializes middlewares.
    /// </summary>
    internal sealed class MiddlewaresInitailizer : IPipelineInitializer
    {
        /// <inheritdoc/>
        public ValueTask RegisterAsync(IPipelineCollection pipelines, CancellationToken cancellationToken)
        {
            _ = PipelineBuilder.Create<CliContext>()
                 .Lifetime(InvokablePipelineLifetime.Scoped)
                 .AddStep<HandleExceptions>()
                 .AddStep<ResolveInput>()
                 .AddStep<ResolveCommand>()
                 .AddStep<InitializeDirectives>()
                 .AddStep<ExecuteDirectivesSubpipeline>()
                 .AddStep<HandleSpecialOptions>()
                 .AddStep<BindInput>()
                 // user
                 .AddStep<ExecuteCommand>()
                 .Build().TryAddTo(pipelines);

            return default;
        }
    }
}
