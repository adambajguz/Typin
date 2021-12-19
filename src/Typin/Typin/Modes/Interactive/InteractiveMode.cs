namespace Typin.Modes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Typin.AutoCompletion;
    using Typin.Console;
    using Typin.Utilities;

    /// <summary>
    /// Interactive CLI mode.
    /// </summary>
    public class InteractiveMode : ICliMode
    {
        private bool firstEnter = true;

        private readonly InteractiveModeOptions _options;
        private readonly IConsole _console;
        private readonly ApplicationMetadata _metadata;
        private readonly ApplicationConfiguration _configuration;
        private readonly ILogger _logger;
        private readonly IServiceProvider _serviceProvider;

        private readonly AutoCompleteInput? _autoCompleteInput;

        /// <summary>
        /// Initializes an instance of <see cref="InteractiveMode"/>.
        /// </summary>
        public InteractiveMode(IOptions<InteractiveModeOptions> options,
                               IConsole console,
                               ILogger<InteractiveMode> logger,
                               IRootSchemaAccessor rootSchemaAccessor,
                               ApplicationMetadata metadata,
                               ApplicationConfiguration configuration,
                               IServiceProvider serviceProvider)
        {
            _options = options.Value;

            _console = console;
            _logger = logger;
            _metadata = metadata;
            _configuration = configuration;
            _serviceProvider = serviceProvider;

            if (_options.IsAdvancedInputAvailable && !_console.Input.IsRedirected)
            {
                _autoCompleteInput = new AutoCompleteInput(_console, _options.UserDefinedShortcuts)
                {
                    AutoCompletionHandler = new AutoCompletionHandler(rootSchemaAccessor),
                };

                _autoCompleteInput.History.IsEnabled = true;
            }
        }

        /// <inheritdoc/>
        public async ValueTask<int> ExecuteAsync(IEnumerable<string> commandLineArguments, ICliCommandExecutor executor)
        {
            if (firstEnter && _configuration.StartupMode == typeof(InteractiveMode) && commandLineArguments.Any())
            {
                await executor.ExecuteCommandAsync(commandLineArguments);
                firstEnter = false;
            }

            IEnumerable<string> interactiveArguments;
            try
            {
                interactiveArguments = await GetInputAsync();
            }
            catch (TaskCanceledException)
            {
                _logger.LogInformation("Interactive mode input cancelled.");
                return ExitCodes.Error;
            }

            _console.ResetColor();

            if (interactiveArguments.Any())
            {
                await executor.ExecuteCommandAsync(interactiveArguments);
                _console.ResetColor();
            }

            return ExitCodes.Success;
        }

        /// <summary>
        /// Gets user input and returns arguments or null if cancelled.
        /// </summary>
        private async Task<IEnumerable<string>> GetInputAsync()
        {
            IConsole console = _console;

            string scope = _options.Scope;
            bool hasScope = !string.IsNullOrWhiteSpace(scope);

            // Print prompt
            _options.Prompt(_serviceProvider, _metadata, _console);

            // Read user input
            console.ForegroundColor = _options.CommandForeground;

            string? line = string.Empty; // Can be null when Ctrl+C is pressed to close the app.
            if (_autoCompleteInput is null)
            {
                line = await console.Input.ReadLineAsync();
            }
            else
            {
                line = await _autoCompleteInput.ReadLineAsync(console.GetCancellationToken());
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
