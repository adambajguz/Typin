namespace Typin.Hosting
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Typin.Console;
    using Typin.Exceptions.Mode;
    using Typin.Modes;
    using Typin.Schemas;
    using Typin.Utilities;

    internal sealed class CliHostService : BackgroundService
    {
        private readonly SemaphoreSlim _startupLock = new(1, 1);

        private readonly IConsole _console;
        private readonly ICliModeSwitcher _cliModeSwitcher;
        private readonly IRootSchemaAccessor _rootSchemaAccessor;
        private readonly ApplicationMetadata _metadata;
        private readonly IOptions<CliOptions> _cliOptions;

        private readonly IHostApplicationLifetime _lifetime;
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
            _lifetime = hostApplicationLifetime;
            _serviceProvider = serviceProvider;
            _logger = loggerFactory.CreateLogger("Typin.Hosting.Diagnostics");
        }

        /// <inheritdoc/>
        protected sealed override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Required for the method to be executed asynchronously, allowing startup to continue.
            //await Task.Yield();
            BeforeStart();

            // Use SemaphoreSlim to run seeder after host startup.
            await _startupLock.WaitAsync(stoppingToken);
            _lifetime.ApplicationStarted.Register(() =>
            {
                if (_startupLock.CurrentCount == 0)
                {
                    _startupLock.Release();
                }
            });

            if (stoppingToken.IsCancellationRequested || _startupLock.CurrentCount != 0)
            {
                return;
            }

            await _startupLock.WaitAsync(stoppingToken);

            await AfterStartAsync(stoppingToken);
        }

        /// <summary>
        /// This method is called before starting the host.
        /// The implementation should return a task that represents the lifetime of the long
        /// running operation(s) being performed.
        /// </summary>
        /// <returns></returns>
        private void BeforeStart()
        {
            _logger.LogInformation("Starting CLI application...");
            _console.ResetColor();

            _ = _rootSchemaAccessor.RootSchema;
        }

        /// <summary>
        /// This method is called after starting the host.
        /// The implementation should return a task that represents the lifetime of the long
        /// running operation(s) being performed.
        /// </summary>
        /// <param name="stoppingToken">Triggered when <see cref="IHostedService.StopAsync(CancellationToken)"/> is called.</param>
        /// <returns></returns>
        private async Task AfterStartAsync(CancellationToken stoppingToken)
        {
            ExitCodeProvider finalExitCodeProvider = new() { ExitCode = ExitCode.Error };

            try
            {
                RootSchema rootSchema = _rootSchemaAccessor.RootSchema;
                CliOptions cliOptions = _cliOptions.Value;

                cliOptions.StartupMessage?.Invoke(_serviceProvider, _metadata, _console);

                await _cliModeSwitcher.WithModeAsync(cliOptions.StartupMode ?? throw new StartupModeNotSetException(), async (mode, ct) =>
                {
                    finalExitCodeProvider.ExitCode = await mode.ExecuteAsync(stoppingToken);

                }, stoppingToken);

                _lifetime.StopApplication();
            }
            catch (Exception ex)
            {
                if (_lifetime.ApplicationStopping.IsCancellationRequested && ex is OperationCanceledException)
                {
                    return;
                }

                _logger.LogCritical(ex, "Fatal error occurred in CLI application. Stopping application...");

                finalExitCodeProvider.ExitCode = ExitCode.Error;

                _console.Error.WithForegroundColor(ConsoleColor.DarkRed, (error) => error.WriteLine($"Unhandled exception. Stopping application..."));
                _console.Error.WriteLine();
                _console.Error.WriteException(ex);

                _lifetime.StopApplication();
            }
            finally
            {
                _logger.LogInformation("Stopping CLI application...");

                IEnumerable<ExitCodeProvider> exitCodeProviders = _serviceProvider.GetServices<ExitCodeProvider>();

                int exitCode = finalExitCodeProvider.ExitCode;
                foreach (ExitCodeProvider e in exitCodeProviders)
                {
                    e.ExitCode = exitCode;
                }
            }
        }
    }
}
