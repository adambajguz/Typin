namespace Typin.Hosting
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Typin.Console;
    using Typin.Exceptions.Mode;
    using Typin.Schemas;
    using Typin.Utilities;

    internal class CliHostService : BackgroundService
    {
        private readonly IConsole _console;
        private readonly ICliModeSwitcher _cliModeSwitcher;
        private readonly IRootSchemaAccessor _rootSchemaAccessor;
        private readonly ApplicationMetadata _metadata;
        private readonly IOptions<CliOptions> _cliOptions;

        private readonly IHostApplicationLifetime _hostApplicationLifetime;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger _logger;

        public CliHostService(IConsole console,
                              ICliModeSwitcher cliModeSwitcher,
                              IOptions<ApplicationMetadata> appMetadataOptions,
                              IOptions<CliOptions> cliOptions,
                              IRootSchemaAccessor rootSchemaAccessor,
                              IHostApplicationLifetime hostApplicationLifetime,
                              IServiceProvider serviceProvider,
                              ILoggerFactory loggerFactory)
        {
            _console = console;
            _cliModeSwitcher = cliModeSwitcher;
            _rootSchemaAccessor = rootSchemaAccessor;
            _metadata = appMetadataOptions.Value;
            _cliOptions = cliOptions;
            _hostApplicationLifetime = hostApplicationLifetime;
            _serviceProvider = serviceProvider;
            _logger = loggerFactory.CreateLogger("Typin.Hosting.Diagnostics");
        }

        ///<inheritdoc/>
        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            // Ensure ExecuteAsync is asynchrojnous by wrapping its body:
            // https://blog.stephencleary.com/2020/05/backgroundservice-gotcha-startup.html
            await Task.Run(async () =>
            {
                try
                {
                    _logger.LogInformation("Starting CLI application...");
                    _console.ResetColor();

                    RootSchema rootSchema = _rootSchemaAccessor.RootSchema;
                    CliOptions cliOptions = _cliOptions.Value;

                    cliOptions.StartupMessage?.Invoke(_serviceProvider, _metadata, _console);

                    await _cliModeSwitcher.WithModeAsync(cliOptions.StartupMode ?? throw new StartupModeNotSetException(), async (mode, ct) =>
                    {
                        int exitCode = await mode.ExecuteAsync(cancellationToken);

                    }, cancellationToken);

                    _logger.LogInformation("Stopping CLI application...");
                    _hostApplicationLifetime.StopApplication();
                }
                catch (Exception ex)
                {
                    if (_hostApplicationLifetime.ApplicationStopping.IsCancellationRequested && ex is OperationCanceledException)
                    {
                        return;
                    }

                    _logger.LogCritical(ex, "Fatal error occurred in CLI application. Stopping application...");

                    _console.Error.WithForegroundColor(ConsoleColor.DarkRed, (error) => error.WriteLine($"Unhandled exception. Stopping application..."));
                    _console.Error.WriteLine();
                    _console.Error.WriteException(ex);

                    _hostApplicationLifetime.StopApplication();
                }
            }, cancellationToken);
        }
    }
}
