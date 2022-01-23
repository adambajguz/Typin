namespace Typin.Modes.Interactive
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using PackSite.Library.Pipelining;
    using Typin;
    using Typin.Commands;
    using Typin.Console;
    using Typin.Modes;
    using Typin.Utilities;

    /// <summary>
    /// Interactive CLI mode.
    /// </summary>
    public sealed class InteractiveMode : ICliMode
    {
        private readonly IOptionsMonitor<CliOptions> _cliOptions;
        private readonly IOptionsMonitor<InteractiveModeOptions> _modeOptions;
        private readonly IConsole _console;
        private readonly IOptionsMonitor<ApplicationMetadata> _metadataOptions;
        private readonly ILogger _logger;
        private readonly ICommandExecutor _commandExecutor;
        private readonly ICliContextAccessor _cliContextAccessor;
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Initializes an instance of <see cref="InteractiveMode"/>.
        /// </summary>
        public InteractiveMode(IOptionsMonitor<CliOptions> cliOptions,
                               IOptionsMonitor<InteractiveModeOptions> modeOptions,
                               IConsole console,
                               ILogger<InteractiveMode> logger,
                               IOptionsMonitor<ApplicationMetadata> metadataOptions,
                               ICommandExecutor commandExecutor,
                               ICliContextAccessor cliContextAccessor,
                               IServiceProvider serviceProvider)
        {
            _cliOptions = cliOptions;
            _modeOptions = modeOptions;
            _console = console;
            _logger = logger;
            _metadataOptions = metadataOptions;
            _commandExecutor = commandExecutor;
            _cliContextAccessor = cliContextAccessor;
            _serviceProvider = serviceProvider;
        }

        /// <inheritdoc/>
        public async Task<int> ExecuteAsync(CancellationToken cancellationToken) //TODO: maybe replace Task<int> with Task
        {
            if (_cliContextAccessor.CliContext.IsStartupContext())
            {
                return await _commandExecutor.ExecuteAsync(_cliOptions.CurrentValue, cancellationToken);
            }

            do
            {
                IEnumerable<string> interactiveArguments;
                try
                {
                    interactiveArguments = await GetInputAsync(cancellationToken);
                }
                catch (OperationCanceledException)
                {
                    _logger.LogInformation("Interactive mode input cancelled.");
                    return ExitCode.Error;
                }

                _console.ResetColor();

                if (interactiveArguments.Any())
                {
                    try
                    {
                        await _commandExecutor.ExecuteAsync(interactiveArguments, InputOptions.Default, ModeBehavior.Default, cancellationToken);
                    }
                    catch (PipelineInvocationException ex)
                    {
                        throw new Exception("Failed to execute.", ex.InnerException); //TODO: replace with custom exception
                    }

                    _console.ResetColor();
                }
            } while (!cancellationToken.IsCancellationRequested);

            return ExitCode.Success;
        }

        /// <summary>
        /// Gets user input and returns arguments or null if cancelled.
        /// </summary>
        private async Task<IEnumerable<string>> GetInputAsync(CancellationToken cancellationToken)
        {
            IConsole console = _console;
            InteractiveModeOptions modeOptionsValue = _modeOptions.CurrentValue;

            // Print prompt
            modeOptionsValue.Prompt(_serviceProvider, _metadataOptions.CurrentValue, _console);

            // Read user input
            console.ForegroundColor = modeOptionsValue.CommandForeground;

            // Line can be null when Ctrl+C is pressed to close the app.
            string? line = await _console.Input.ReadLineAsync().WaitAsync(cancellationToken);

            console.ForegroundColor = ConsoleColor.Gray;

            return !string.IsNullOrWhiteSpace(line)
                ? CommandLine.Split(line)
                : Enumerable.Empty<string>();
        }
    }
}
