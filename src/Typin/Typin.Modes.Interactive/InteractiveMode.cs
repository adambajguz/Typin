namespace Typin.Modes.Interactive
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Typin;
    using Typin.Console;
    using Typin.Modes.Interactive.AutoCompletion;
    using Typin.Modes.Interactive.Internal.Extensions;
    using Typin.Utilities;

    /// <summary>
    /// Interactive CLI mode.
    /// </summary>
    public class InteractiveMode : ICliMode
    {
        private readonly IOptionsMonitor<InteractiveModeOptions> _modeOptions;
        private readonly CliOptions _cliOptions;
        private readonly IConsole _console;
        private readonly IOptionsMonitor<ApplicationMetadata> _metadataOptions;
        private readonly ILogger _logger;
        private readonly IServiceProvider _serviceProvider;

        private readonly AutoCompleteInput? _autoCompleteInput;

        /// <summary>
        /// Initializes an instance of <see cref="InteractiveMode"/>.
        /// </summary>
        public InteractiveMode(IOptionsMonitor<InteractiveModeOptions> modeOptions,
                               IOptions<CliOptions> cliOptions,
                               IConsole console,
                               ILogger<InteractiveMode> logger,
                               IRootSchemaAccessor rootSchemaAccessor,
                               IOptionsMonitor<ApplicationMetadata> metadataOptions,
                               IServiceProvider serviceProvider)
        {
            _modeOptions = modeOptions;
            _cliOptions = cliOptions.Value;

            _console = console;
            _logger = logger;
            _metadataOptions = metadataOptions;
            _serviceProvider = serviceProvider;

            InteractiveModeOptions modeOptionsValue = _modeOptions.CurrentValue;
            if (modeOptionsValue.IsAdvancedInputAvailable && !console.Input.IsRedirected)
            {
                _autoCompleteInput = new AutoCompleteInput(_console, modeOptionsValue.UserDefinedShortcuts)
                {
                    AutoCompletionHandler = new AutoCompletionHandler(rootSchemaAccessor),
                };

                _autoCompleteInput.History.IsEnabled = true;
            }
        }

        /// <inheritdoc/>
        public async ValueTask<int> ExecuteAsync(ICliCommandExecutor executor, bool isStartupContext, CancellationToken cancellationToken)
        {
            if (isStartupContext)
            {
                await executor.ExecuteCommandAsync(
                    _cliOptions.CommandLine ?? string.Empty,
                    _cliOptions.CommandLineStartsWithExecutableName,
                    cancellationToken);
            }

            do
            {
                IEnumerable<string> interactiveArguments;
                try
                {
                    interactiveArguments = await GetInputAsync(cancellationToken);
                }
                catch (TaskCanceledException)
                {
                    _logger.LogInformation("Interactive mode input cancelled.");
                    return ExitCode.Error;
                }

                _console.ResetColor();

                if (interactiveArguments.Any())
                {
                    await executor.ExecuteCommandAsync(interactiveArguments, false, cancellationToken);
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

            string scope = modeOptionsValue.Scope;
            bool hasScope = !string.IsNullOrWhiteSpace(scope);

            // Print prompt
            modeOptionsValue.Prompt(_serviceProvider, _metadataOptions.CurrentValue, _console);

            // Read user input
            console.ForegroundColor = modeOptionsValue.CommandForeground;

            string? line = string.Empty; // Can be null when Ctrl+C is pressed to close the app.
            if (_autoCompleteInput is null)
            {
                line = await _console.Input.ReadLineAsync().WithCancellation(cancellationToken);
            }
            else
            {
                line = await _autoCompleteInput.ReadLineAsync(cancellationToken);
            }

            console.ForegroundColor = ConsoleColor.Gray;

            IEnumerable<string> arguments = Enumerable.Empty<string>();

            if (!string.IsNullOrWhiteSpace(line))
            {
                if (hasScope) // handle scoped command input
                {
                    List<string> tmp = CommandLineSplitter.Split(line).ToList();

                    int lastDirective = tmp.FindLastIndex(x => x.StartsWith('[') && x.EndsWith(']'));
                    tmp.Insert(lastDirective + 1, scope);

                    arguments = tmp.ToArray();
                }
                else // handle unscoped command input
                {
                    arguments = CommandLineSplitter.Split(line);
                }
            }

            return arguments;
        }
    }
}
