namespace Typin
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Typin.Console;
    using Typin.Exceptions;
    using Typin.Internal;
    using Typin.Utilities;

    /// <summary>
    /// Command line application facade.
    /// </summary>
    public sealed class CliApplication
    {
        private readonly IServiceProvider _serviceProvider;

        private readonly ApplicationMetadata _metadata;
        private readonly IConsole _console;
        private readonly ICliCommandExecutor _cliCommandExecutor;
        private readonly CliApplicationLifetime _applicationLifetime;
        private readonly EnvironmentVariablesAccessor _environmentVariablesAccessor;
        private readonly ILogger _logger;

        /// <summary>
        /// Initializes an instance of <see cref="CliApplication"/>.
        /// </summary>
        internal CliApplication(IServiceProvider serviceProvider,
                                IConsole console,
                                EnvironmentVariablesAccessor environmentVariablesAccessor,
                                ApplicationMetadata metadata)
        {
            _serviceProvider = serviceProvider;

            _environmentVariablesAccessor = environmentVariablesAccessor;
            _metadata = metadata;
            _console = console;

            _logger = serviceProvider.GetRequiredService<ILogger<CliApplication>>();
            _cliCommandExecutor = serviceProvider.GetRequiredService<ICliCommandExecutor>();
            _applicationLifetime = (CliApplicationLifetime)serviceProvider.GetRequiredService<ICliApplicationLifetime>();
        }

        /// <summary>
        /// Prints the startup message if availble.
        /// </summary>
        private void PrintStartupMessage()
        {
            if (_metadata.StartupMessage is null)
                return;

            _console.Output.WithForegroundColor(ConsoleColor.Blue, (output) => output.WriteLine(_metadata.StartupMessage));
        }

        /// <summary>
        /// Runs the application and returns the exit code.
        /// Command line arguments and environment variables are retrieved automatically.
        /// </summary>
        /// <remarks>
        /// If a <see cref="CommandException"/>, <see cref="DirectiveException"/>, or <see cref="TypinException"/> is thrown during command execution,
        /// it will be handled and routed to the console. Additionally, if the debugger is not attached (i.e. the app is running in production),
        /// all other exceptions thrown within this method will be handled and routed to the console as well.
        /// </remarks>
        public async ValueTask<int> RunAsync()
        {
            string line = Environment.CommandLine;

            return await RunAsync(line, true);
        }

        /// <summary>
        /// Runs the application with specified command line and returns the exit code.
        /// Environment variables are retrieved automatically.
        /// </summary>
        /// <remarks>
        /// If a <see cref="CommandException"/>, <see cref="DirectiveException"/>, or <see cref="TypinException"/> is thrown during command execution,
        /// it will be handled and routed to the console. Additionally, if the debugger is not attached (i.e. the app is running in production),
        /// all other exceptions thrown within this method will be handled and routed to the console as well.
        /// </remarks>
        public async ValueTask<int> RunAsync(string commandLine, bool containsExecutable = false)
        {
            IEnumerable<string> commandLineArguments = CommandLineSplitter.Split(commandLine)
                                                                          .Skip(containsExecutable ? 1 : 0);

            return await RunAsync(commandLineArguments);
        }

        /// <summary>
        /// Runs the application with specified command line and environment variables, and returns the exit code.
        /// </summary>
        /// <remarks>
        /// If a <see cref="CommandException"/>, <see cref="DirectiveException"/>, or <see cref="TypinException"/> is thrown during command execution,
        /// it will be handled and routed to the console. Additionally, if the debugger is not attached (i.e. the app is running in production),
        /// all other exceptions thrown within this method will be handled and routed to the console as well.
        /// </remarks>
        public async ValueTask<int> RunAsync(string commandLine, IReadOnlyDictionary<string, string> environmentVariables, bool containsExecutable = false)
        {
            IEnumerable<string> commandLineArguments = CommandLineSplitter.Split(commandLine)
                                                                          .Skip(containsExecutable ? 1 : 0);

            return await RunAsync(commandLineArguments, environmentVariables);
        }

        /// <summary>
        /// Runs the application with specified command line arguments and returns the exit code.
        /// Environment variables are retrieved automatically.
        /// </summary>
        /// <remarks>
        /// If a <see cref="CommandException"/>, <see cref="DirectiveException"/>, or <see cref="TypinException"/> is thrown during command execution,
        /// it will be handled and routed to the console. Additionally, if the debugger is not attached (i.e. the app is running in production),
        /// all other exceptions thrown within this method will be handled and routed to the console as well.
        /// </remarks>
        public async ValueTask<int> RunAsync(IEnumerable<string> commandLineArguments)
        {
            // Environment variable names are case-insensitive on Windows but are case-sensitive on Linux and macOS
            Dictionary<string, string> environmentVariables = Environment.GetEnvironmentVariables()
                                                                         .Cast<DictionaryEntry>()
                                                                         .ToDictionary(x => (string)x.Key,
                                                                                       x => (x.Value as string) ?? string.Empty,
                                                                                       StringComparer.Ordinal);

            return await RunAsync(commandLineArguments, environmentVariables);
        }


        /// <summary>
        /// Runs the application with specified command line arguments and environment variables, and returns the exit code.
        /// </summary>
        /// <remarks>
        /// If a <see cref="CommandException"/>, <see cref="DirectiveException"/>, or <see cref="TypinException"/> is thrown during command execution,
        /// it will be handled and routed to the console. Additionally, if the debugger is not attached (i.e. the app is running in production),
        /// all other exceptions thrown within this method will be handled and routed to the console as well.
        /// </remarks>
        [SuppressMessage("Design", "CA1031:Do not catch general exception types")]
        public async ValueTask<int> RunAsync(IEnumerable<string> commandLineArguments,
                                             IReadOnlyDictionary<string, string> environmentVariables)
        {
            try
            {
                _logger.LogInformation("Starting CLI application...");
                _console.ResetColor();

                _environmentVariablesAccessor.EnvironmentVariables = environmentVariables;

                //TODO: OnStart()

                PrintStartupMessage();

                int exitCode = await StartAppAsync(commandLineArguments);

                //TODO: OnStop()
                _logger.LogInformation("CLI application stopped.");

                return exitCode;
            }
            // This may throw pre-execution resolving exceptions which are useful only to the end-user
            catch (TypinException ex)
            {
                _logger.LogDebug(ex, $"{nameof(TypinException)} occured. Trying to find exception handler.");

                IEnumerable <ICliExceptionHandler> exceptionHandlers = _serviceProvider.GetServices<ICliExceptionHandler>();
                foreach (ICliExceptionHandler handler in exceptionHandlers)
                {
                    if (handler.HandleException(ex))
                    {
                        _logger.LogDebug(ex, "Exception handled by {ExceptionHandlerType}.", handler.GetType().FullName);

                        break;
                    }
                }

                _logger.LogCritical(ex, "Unhandled Typin exception caused app to terminate.");

                return ExitCodes.FromException(ex);
            }
            // To prevent the app from showing the annoying Windows troubleshooting dialog,
            // we handle all exceptions and route them to the console nicely.
            // However, we don't want to swallow unhandled exceptions when the debugger is attached,
            // because we still want the IDE to show them to the developer.
            catch (Exception ex) //when (!Debugger.IsAttached)
            {
                _logger.LogCritical(ex, "Unhandled exception caused app to terminate.");

                _console.Error.WithForegroundColor(ConsoleColor.DarkRed, (error) => error.WriteLine($"Fatal error occured in {_metadata.ExecutableName}."));

                _console.Error.WriteLine();
                _console.Error.WithForegroundColor(ConsoleColor.DarkRed, (error) => error.WriteLine(ex.ToString()));
                _console.Error.WriteLine();

                return ExitCodes.FromException(ex);
            }
        }

        private async Task<int> StartAppAsync(IEnumerable<string> commandLineArguments)
        {
            _applicationLifetime.Initialize();

            int exitCode = ExitCodes.Error;
            CancellationToken cancellationToken = _console.GetCancellationToken();

            while (_applicationLifetime.State == CliLifetimes.Running && !cancellationToken.IsCancellationRequested)
            {
                ICliMode? currentMode = _applicationLifetime.CurrentMode;

                if (currentMode != null)
                    exitCode = await currentMode.ExecuteAsync(commandLineArguments, _cliCommandExecutor);

                _applicationLifetime.TrySwitchModes();
                _applicationLifetime.TryStop();
            }

            _logger.LogInformation("CLI application will stop with '{ExitCode}'.", exitCode);

            _applicationLifetime.State = CliLifetimes.Stopped;

            return exitCode;
        }
    }
}
