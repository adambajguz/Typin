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
    using Typin.Console;
    using Typin.Extensions;
    using Typin.Modes.Interactive.AutoCompletion;
    using Typin.Modes.Interactive.Internal.Extensions;
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

        private readonly AutoCompleteInput? _autoCompleteInput;

        /// <summary>
        /// Initializes an instance of <see cref="InteractiveMode"/>.
        /// </summary>
        public InteractiveMode(IOptionsMonitor<CliOptions> cliOptions,
                               IOptionsMonitor<InteractiveModeOptions> modeOptions,
                               IConsole console,
                               ILogger<InteractiveMode> logger,
                               IRootSchemaAccessor rootSchemaAccessor,
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
                        await _commandExecutor.ExecuteAsync(interactiveArguments, CommandExecutionOptions.Default, cancellationToken);
                    }
                    catch (PipelineInvocationException ex)
                    {
                        throw new Exception("Failed to execute pipelined directives.", ex.InnerException); //TODO: replace with custom exception
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
                    List<string> tmp = CommandLine.Split(line).ToList();

                    int lastDirective = tmp.FindLastIndex(x => x.StartsWith('[') && x.EndsWith(']'));
                    tmp.Insert(lastDirective + 1, scope);

                    arguments = tmp.ToArray();
                }
                else // handle unscoped command input
                {
                    arguments = CommandLine.Split(line);
                }
            }

            return arguments;
        }
    }
}
