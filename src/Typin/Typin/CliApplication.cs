namespace Typin
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using Typin.Console;
    using Typin.Exceptions;
    using Typin.Internal;
    using Typin.Internal.Schemas;

    /// <summary>
    /// Command line application facade.
    /// </summary>
    public sealed class CliApplication
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly CliContextFactory _cliContextFactory;

        private readonly ApplicationConfiguration _configuration;
        private readonly ApplicationMetadata _metadata;
        private readonly IConsole _console;
        private readonly ICliCommandExecutor _cliCommandExecutor;
        private readonly CliModeSwitcher _cliModeSwitcher;

        /// <summary>
        /// Initializes an instance of <see cref="CliApplication"/>.
        /// </summary>
        internal CliApplication(IServiceProvider serviceProvider, CliContextFactory cliContextFactory)
        {
            _serviceProvider = serviceProvider;
            _cliContextFactory = cliContextFactory;

            _configuration = serviceProvider.GetService<ApplicationConfiguration>();
            _metadata = serviceProvider.GetService<ApplicationMetadata>();
            _console = serviceProvider.GetService<IConsole>();
            _cliCommandExecutor = serviceProvider.GetService<ICliCommandExecutor>();
            _cliModeSwitcher = (CliModeSwitcher)serviceProvider.GetService<ICliModeSwitcher>();
        }

        /// <summary>
        /// Prints the startup message if availble.
        /// </summary>
        private void PrintStartupMessage()
        {
            if (_metadata.StartupMessage is null)
                return;

            _console.WithForegroundColor(ConsoleColor.Blue, () => _console.Output.WriteLine(_metadata.StartupMessage));
        }

        #region Run
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
            string[] commandLineArguments = CommandLineSplitter.Split(commandLine)
                                                               .Skip(containsExecutable ? 1 : 0)
                                                               .ToArray();

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
            string[] commandLineArguments = CommandLineSplitter.Split(commandLine)
                                                               .Skip(containsExecutable ? 1 : 0)
                                                               .ToArray();

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
        public async ValueTask<int> RunAsync(IReadOnlyList<string> commandLineArguments)
        {
            // Environment variable names are case-insensitive on Windows but are case-sensitive on Linux and macOS
            Dictionary<string, string> environmentVariables = Environment.GetEnvironmentVariables()
                                                                         .Cast<DictionaryEntry>()
                                                                         .ToDictionary(x => (string)x.Key,
                                                                                       x => (string)x.Value,
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
        public async ValueTask<int> RunAsync(IReadOnlyList<string> commandLineArguments,
                                             IReadOnlyDictionary<string, string> environmentVariables)
        {
            try
            {
                _console.ResetColor();
                _console.ForegroundColor = ConsoleColor.Gray;

                _cliContextFactory.EnvironmentVariables = environmentVariables;
                _cliContextFactory.RootSchema = new RootSchemaResolver(_configuration.CommandTypes, _configuration.DirectiveTypes, _configuration.ModeTypes).Resolve();

                //TODO: OnStart()

                PrintStartupMessage();

                //TODO: when in commandLineArguments is a string.Empty application crashes
                int exitCode = await StartAppAsync(commandLineArguments);

                //TODO: OnStop()

                return exitCode;
            }
            // This may throw pre-execution resolving exceptions which are useful only to the end-user
            catch (TypinException ex)
            {
                IEnumerable<ICliExceptionHandler> exceptionHandlers = _serviceProvider.GetServices<ICliExceptionHandler>();
                foreach (ICliExceptionHandler handler in exceptionHandlers)
                {
                    if (handler.HandleException(ex))
                        break;
                }

                return ExitCodes.FromException(ex);
            }
            // To prevent the app from showing the annoying Windows troubleshooting dialog,
            // we handle all exceptions and route them to the console nicely.
            // However, we don't want to swallow unhandled exceptions when the debugger is attached,
            // because we still want the IDE to show them to the developer.
            catch (Exception ex) when (!Debugger.IsAttached)
            {
                _console.WithForegroundColor(ConsoleColor.DarkRed, () => _console.Error.WriteLine($"Fatal error occured in {_metadata.ExecutableName}."));

                _console.Error.WriteLine();
                _console.WithForegroundColor(ConsoleColor.DarkRed, () => _console.Error.WriteLine(ex.ToString()));
                _console.Error.WriteLine();

                return ExitCodes.FromException(ex);
            }
        }
        #endregion

        private async Task<int> StartAppAsync(IReadOnlyList<string> commandLineArguments)
        {
            //Perform startup mode switching.
            _cliModeSwitcher.TrySwitchModes();

            int exitCode = ExitCodes.Error;

            bool isRunning = true;
            while (isRunning && !_console.GetCancellationToken().IsCancellationRequested)
            {
                exitCode = await _cliModeSwitcher.Current!.Execute(commandLineArguments, _cliCommandExecutor);

                _cliModeSwitcher.TrySwitchModes();
                isRunning = false;
            }

            return exitCode;
        }
    }
}
