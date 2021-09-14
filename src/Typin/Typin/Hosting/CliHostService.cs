namespace Typin.Hosting
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Typin.Console;
    using Typin.Modes;
    using Typin.Schemas;
    using Typin.Utilities;

    internal class CliHostService : BackgroundService
    {
        private readonly IConsole _console;
        private readonly ICliCommandExecutor _cliCommandExecutor;
        private readonly IRootSchemaAccessor _rootSchemaAccessor;
        private readonly ApplicationMetadata _metadata;
        private readonly IOptions<CliOptions> _cliOptions;

        private readonly IHostEnvironment _hostingEnvironment;
        private readonly IHostApplicationLifetime _hostApplicationLifetime;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger _logger;

        public CliHostService(IHostEnvironment hostingEnvirnment,
                              IConsole console,
                              ICliCommandExecutor cliCommandExecutor,
                              IOptions<ApplicationMetadata> appMetadataOptions,
                              IOptions<CliOptions> cliOptions,
                              IRootSchemaAccessor rootSchemaAccessor,
                              IHostApplicationLifetime hostApplicationLifetime,
                              IServiceProvider serviceProvider,
                              ILoggerFactory loggerFactory)
        {
            _console = console;
            _cliCommandExecutor = cliCommandExecutor;
            _rootSchemaAccessor = rootSchemaAccessor;
            _metadata = appMetadataOptions.Value;
            _cliOptions = cliOptions;

            _hostingEnvironment = hostingEnvirnment;
            _hostApplicationLifetime = hostApplicationLifetime;
            _serviceProvider = serviceProvider;
            _logger = loggerFactory.CreateLogger("Typin.Hosting.Diagnostics");
        }

        ///<inheritdoc/>
        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Starting CLI application...");
                _console.ResetColor();

                RootSchema rootSchema = _rootSchemaAccessor.RootSchema;

                _cliOptions.Value.StartupMessage?.Invoke(_serviceProvider, _metadata, _console);

                Type startupModeType = _cliOptions.Value.StartupMode ?? typeof(DirectMode);
                ICliMode startupMode = (ICliMode)_serviceProvider.GetRequiredService(startupModeType);

                int exitCode = await startupMode.ExecuteAsync(_cliCommandExecutor, true, cancellationToken);
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
        }
    }
}
