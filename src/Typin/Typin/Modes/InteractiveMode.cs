namespace Typin.Modes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Options;
    using Typin.AutoCompletion;
    using Typin.Console;
    using Typin.Internal;

    /// <summary>
    /// Interactive CLI mode.
    /// </summary>
    public class InteractiveMode : ICliMode
    {
        private readonly bool firstEnter = true;

        private readonly IConsole _console;
        private readonly ApplicationMetadata _metadata;

        private readonly AutoCompleteInput? _autoCompleteInput;

        /// <summary>
        /// Mode options.
        /// </summary>
        public InteractiveModeSettings Options { get; }

        /// <summary>
        /// Initializes an instance of <see cref="InteractiveMode"/>.
        /// </summary>
        public InteractiveMode(IOptions<InteractiveModeSettings> options, IConsole console, ApplicationMetadata metadata)
        {
            Options = options.Value;

            _console = console;
            _metadata = metadata;

            //TODO: fix advanced mode
            if (Options.IsAdvancedInputAvailable && !console.Input.IsRedirected)
            {
                //_autoCompleteInput = new AutoCompleteInput(console, Options.UserDefinedShortcut)
                //{
                //    AutoCompletionHandler = new AutoCompletionHandler(cliContext),
                //};

                //_autoCompleteInput.History.IsEnabled = true;
                //cliContext.InternalInputHistory = _autoCompleteInput.History;
            }
        }

        /// <inheritdoc/>
        public async ValueTask<int> Execute(IReadOnlyList<string> commandLineArguments, ICliCommandExecutor executor)
        {
            //TODO: fix advanced mode execution
            if (firstEnter)
            {
                //await executor.ExecuteCommand(commandLineArguments);
            }

            string[]? interactiveArguments = await GetInputAsync(_console, _metadata.ExecutableName);

            if (interactiveArguments is null)
            {
                _console.ResetColor();
                return ExitCodes.Success;
            }

            await executor.ExecuteCommandAsync(interactiveArguments);
            _console.ResetColor();

            return ExitCodes.Success;
        }

        /// <summary>
        /// Gets user input and returns arguments or null if cancelled.
        /// </summary>
        private async Task<string[]?> GetInputAsync(IConsole console, string executableName)
        {
            string[]? arguments = null;
            string? line = string.Empty; // Can be null when Ctrl+C is pressed to close the app.

            ConsoleColor promptForeground = Options.PromptForeground;
            ConsoleColor commandForeground = Options.CommandForeground;

            // Print prompt
            console.WithForegroundColor(promptForeground, () =>
            {
                console.Output.Write(executableName);
            });

            string scope = Options.Scope;

            if (!string.IsNullOrWhiteSpace(scope))
            {
                console.WithForegroundColor(ConsoleColor.Cyan, () =>
                {
                    console.Output.Write(' ');
                    console.Output.Write(scope);
                });
            }

            console.WithForegroundColor(promptForeground, () =>
            {
                console.Output.Write("> ");
            });

            // Read user input
            ConsoleColor lastColor = console.ForegroundColor;
            console.ForegroundColor = commandForeground;

            if (_autoCompleteInput is null)
                line = await console.Input.ReadLineAsync();
            else
                line = await _autoCompleteInput.ReadLineAsync();

            console.ForegroundColor = lastColor;

            if (!string.IsNullOrWhiteSpace(line))
            {
                if (string.IsNullOrWhiteSpace(scope)) // handle unscoped command input
                {
                    arguments = CommandLineSplitter.Split(line)
                                                   .Where(x => !string.IsNullOrWhiteSpace(x))
                                                   .ToArray();
                }
                else // handle scoped command input
                {
                    List<string> tmp = CommandLineSplitter.Split(line)
                                                          .Where(x => !string.IsNullOrWhiteSpace(x))
                                                          .ToList();

                    int lastDirective = tmp.FindLastIndex(x => x.StartsWith('[') && x.EndsWith(']'));
                    tmp.Insert(lastDirective + 1, scope);

                    arguments = tmp.ToArray();
                }
            }

            console.ForegroundColor = ConsoleColor.Gray;

            return arguments;
        }
    }
}
