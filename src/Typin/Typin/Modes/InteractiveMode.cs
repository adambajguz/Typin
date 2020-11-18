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

            await RunInteractivelyAsync(executor);

            return ExitCodes.Success;
        }

        private async Task RunInteractivelyAsync(ICliCommandExecutor executor)
        {
            string[]? commandLineArguments = GetInput(_console, _metadata.ExecutableName);

            if (commandLineArguments is null)
            {
                _console.ResetColor();
                return;
            }

            await executor.ExecuteCommand(commandLineArguments);
            _console.ResetColor();
        }

        /// <summary>
        /// Gets user input and returns arguments or null if cancelled.
        /// </summary>
        private string[]? GetInput(IConsole console, string executableName)
        {
            string[] arguments;
            string? line = string.Empty; // Can be null when Ctrl+C is pressed to close the app.

            do
            {
                ConsoleColor promptForeground = Options.PromptForeground;
                ConsoleColor commandForeground = Options.CommandForeground;

                // Print prompt
                console.WithForegroundColor(promptForeground, () =>
                {
                    console.Output.Write(executableName);
                });

                string scope = string.Empty;//CliContext.Scope;

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
                console.WithForegroundColor(commandForeground, () =>
                {
                    if (_autoCompleteInput is null)
                        line = console.Input.ReadLine();
                    else
                        line = _autoCompleteInput.ReadLine();
                });

                if (line is null)
                    return null;

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

            } while (string.IsNullOrWhiteSpace(line)); // retry on empty line

            console.ForegroundColor = ConsoleColor.Gray;

            return arguments;
        }
    }
}
