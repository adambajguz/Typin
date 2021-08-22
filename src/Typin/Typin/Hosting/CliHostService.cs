namespace Typin.Hosting
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using PackSite.Library.Pipelining;
    using Typin.Internal.Pipeline;

    internal class CliHostService : BackgroundService
    {
        private readonly ILogger _logger;
        private readonly IHostEnvironment _hostingEnvironment;
        private readonly IHostApplicationLifetime _hostApplicationLifetime;
        private readonly IPipelineCollection _pipelineCollection;

        public CliHostService(ILoggerFactory loggerFactory,
                              IHostEnvironment hostingEnvirnment,
                              IHostApplicationLifetime hostApplicationLifetime,
                              IPipelineCollection pipelineCollection)
        {
            _logger = loggerFactory.CreateLogger("Typin.Hosting.Diagnostics");
            _hostingEnvironment = hostingEnvirnment;
            _hostApplicationLifetime = hostApplicationLifetime;
            _pipelineCollection = pipelineCollection;
        }

        ///<inheritdoc/>
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var pipelineBuilder = PipelineBuilder.Create<ICliContext>()
                .Lifetime(InvokablePipelineLifetime.Scoped)
                .Add<ResolveCommandSchemaAndInstance>()
                .Add<InitializeDirectives>()
                .Add<ExecuteDirectivesSubpipeline>()
                .Add<HandleSpecialOptions>()
                .Add<BindInput>()
                // user
                .Add<ExecuteCommand>()
                .Build().TryAddTo(_pipelineCollection);

            throw new NotImplementedException();
        }
    }
}
