namespace Typin
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;
    using Typin.Hosting;

    /// <summary>
    /// Command line application facade.
    /// </summary>
    public sealed class CliApplication
    {
        private readonly IHostBuilder _hostBuilder;
        private readonly ExitCodeProvider _exitCodeProvider = new();

        /// <summary>
        /// Initializes an instance of <see cref="CliApplication"/>.
        /// </summary>
        internal CliApplication(IHostBuilder hostBuilder, ExitCodeProvider exitCodeProvider)
        {
            _hostBuilder = hostBuilder;
            _exitCodeProvider = exitCodeProvider;
        }

        /// <summary>
        /// Runs the application and returns the exit code.
        /// Command line arguments and environment variables are retrieved automatically.
        /// </summary>
        public async ValueTask<int> RunAsync()
        {
            await _hostBuilder.RunConsoleAsync();

            return _exitCodeProvider.ExitCode;
        }

        /// <summary>
        /// Runs the application with specified command line and returns the exit code.
        /// Environment variables are retrieved automatically.
        /// </summary>
        public async ValueTask<int> RunAsync(string commandLine, bool containsExecutable = false)
        {
            return await RunAsync(commandLine, environmentVariables: null!, containsExecutable);
        }

        /// <summary>
        /// Runs the application with specified command line and environment variables, and returns the exit code.
        /// </summary>
        public async ValueTask<int> RunAsync(string commandLine, IReadOnlyDictionary<string, string> environmentVariables, bool containsExecutable = false)
        {
            _hostBuilder.ConfigureCliHost(cliBuilder =>
            {
                cliBuilder.OverrideCommandLine(commandLine,
                                               containsExecutable ? InputOptions.TrimExecutable : InputOptions.Default);
            });

            AddLegacyEnvVars(environmentVariables);

            await _hostBuilder.RunConsoleAsync();

            return _exitCodeProvider.ExitCode;
        }

        /// <summary>
        /// Runs the application with specified command line arguments and returns the exit code.
        /// Environment variables are retrieved automatically.
        /// </summary>
        public async ValueTask<int> RunAsync(IEnumerable<string> commandLineArguments)
        {
            return await RunAsync(commandLineArguments, null!);
        }

        /// <summary>
        /// Runs the application with specified command line arguments and environment variables, and returns the exit code.
        /// </summary>
        public async ValueTask<int> RunAsync(IEnumerable<string> commandLineArguments,
                                             IReadOnlyDictionary<string, string> environmentVariables)
        {
            _hostBuilder.ConfigureCliHost(cliBuilder =>
            {
                cliBuilder.OverrideCommandLine(commandLineArguments);
            });

            AddLegacyEnvVars(environmentVariables);

            await _hostBuilder.RunConsoleAsync();

            return _exitCodeProvider.ExitCode;
        }

        private void AddLegacyEnvVars(IReadOnlyDictionary<string, string> environmentVariables)
        {
            if (environmentVariables is { Count: > 0 })
            {
                _hostBuilder.ConfigureAppConfiguration((context, builder) =>
                {
                    builder.AddInMemoryCollection(environmentVariables);
                });
            }
        }
    }
}