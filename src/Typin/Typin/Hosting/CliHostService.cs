namespace Typin.Hosting
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Typin.Hosting.Startup;

    internal class CliHostService : IHostedService
    {
        private readonly CliHostServiceOptions _options;

        private readonly ILogger _logger;
        private readonly IApplicationBuilderFactory _applicationBuilderFactory;
        private readonly IEnumerable<IStartupFilter> _startupFilters;
        private readonly IConfiguration _configuration;
        private readonly IHostEnvironment _hostingEnvirnment;
        private readonly IHostApplicationLifetime _hostApplicationLifetime;
        private readonly CliApplication _cliApplication;

        public CliHostService(IOptions<CliHostServiceOptions> options,
                              ILoggerFactory loggerFactory,
                              IApplicationBuilderFactory applicationBuilderFactory,
                              IEnumerable<IStartupFilter> startupFilters,
                              IConfiguration configuration,
                              IHostEnvironment hostingEnvironment,
                              IHostApplicationLifetime hostApplicationLifetime,
                              CliApplication cliApplication)
        {
            _options = options.Value;
            _logger = loggerFactory.CreateLogger("Typin.Hosting.Diagnostics");
            _applicationBuilderFactory = applicationBuilderFactory;
            _startupFilters = startupFilters;
            _configuration = configuration;
            _hostingEnvirnment = hostingEnvironment;
            _hostApplicationLifetime = hostApplicationLifetime;
            _cliApplication = cliApplication;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                var configure = _options.ConfigureApplication;

                if (configure == null)
                {
                    throw new InvalidOperationException($"No application configured. Please specify an application via in the CLI host configuration.");
                }

                var builder = _applicationBuilderFactory.CreateBuilder();

                foreach (var filter in _startupFilters.Reverse())
                {
                    configure = filter.Configure(configure);
                }

                configure(builder);

                // Build the request pipeline
                builder.Build();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "s");
            }

            _hostApplicationLifetime.ApplicationStarted.Register(async () =>
            {
                await _cliApplication.RunAsync(cancellationToken);
            });

            //if (Options.HostingStartupExceptions != null)
            //{
            //    foreach (var exception in Options.HostingStartupExceptions.InnerExceptions)
            //    {
            //        Logger.HostingStartupAssemblyError(exception);
            //    }
            //}
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {

        }
    }
}
