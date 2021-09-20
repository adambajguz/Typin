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
                 .Step<HandleExceptions>()
                 .Step<ResolveInput>()
                 .Step<ResolveCommand>()
                 .Step<InitializeDirectives>()
                 .Step<ExecuteDirectivesSubpipeline>()
                 .Step<HandleSpecialOptions>()
                 .Step<BindInput>()
                 // user
                 .Step<ExecuteCommand>()
                 .Build().TryAddTo(pipelines);

            return default;
        }
    }
}
