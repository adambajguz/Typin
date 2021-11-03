namespace Typin.Internal.Input
{
    using System.Collections.Generic;
    using System.Linq;
    using Typin.Input;
    using Typin.Internal.Extensions;

    /// <summary>
    /// Resolves an instance of <see cref="ParsedCommandInput"/>.
    /// </summary>
    internal class InputResolver
    {
        /// <summary>
        /// Resolves <see cref="ParsedCommandInput"/>.
        /// </summary>
        public static ParsedCommandInput Parse(IEnumerable<string> commandLineArguments,
                                         ISet<string> availableCommandNamesSet)
        {
            int index = 0;

            IReadOnlyList<string> tmp = commandLineArguments.ToList();

            IReadOnlyList<DirectiveInput> directives = ParseDirectives(
                tmp,
                ref index
            );

            string? commandName = ParseCommandName(
                tmp,
                availableCommandNamesSet,
                ref index
            );

            IReadOnlyList<CommandParameterInput> parameters = ParseParameters(
                tmp,
                ref index
            );

            IReadOnlyList<CommandOptionInput> options = ParseOptions(
                tmp,
                ref index
            );

            return new ParsedCommandInput(directives, commandName, parameters, options);
        }

        private static IReadOnlyList<DirectiveInput> ParseDirectives(IReadOnlyList<string> commandLineArguments,
                                                                     ref int index)
        {
            List<DirectiveInput> result = new();

            for (; index < commandLineArguments.Count; index++)
            {
                string argument = commandLineArguments[index];

                if (!argument.StartsWith('[') || !argument.EndsWith(']'))
                {
                    break;
                }

                string name = argument[1..^1];

                result.Add(new DirectiveInput(name));
            }

            return result;
        }

        private static string? ParseCommandName(IReadOnlyList<string> commandLineArguments,
                                                ISet<string> commandNames,
                                                ref int index)
        {
            List<string> buffer = new();

            string? commandName = null;
            int lastIndex = index;

            // We need to look ahead to see if we can match as many consecutive arguments to a command name as possible
            for (int i = index; i < commandLineArguments.Count; ++i)
            {
                string argument = commandLineArguments[i];
                buffer.Add(argument);

                string potentialCommandName = buffer.JoinToString(' ');

                if (commandNames.Contains(potentialCommandName))
                {
                    commandName = potentialCommandName;
                    lastIndex = i;
                }
            }

            // Update the index only if command name was found in the arguments
            if (!string.IsNullOrWhiteSpace(commandName))
            {
                index = lastIndex + 1;
            }

            return commandName;
        }

        private static IReadOnlyList<CommandParameterInput> ParseParameters(IReadOnlyList<string> commandLineArguments,
                                                                         ref int index)
        {
            List<CommandParameterInput> result = new();

            for (; index < commandLineArguments.Count; index++)
            {
                string argument = commandLineArguments[index];

                if (CommandOptionInput.IsOption(argument) || CommandOptionInput.IsOptionAlias(argument))
                {
                    break;
                }

                result.Add(new CommandParameterInput(argument));
            }

            return result;
        }

        private static IReadOnlyList<CommandOptionInput> ParseOptions(IReadOnlyList<string> commandLineArguments,
                                                                      ref int index)
        {
            List<CommandOptionInput> result = new();

            string? currentOptionAlias = null;
            List<string> currentOptionValues = new();

            for (; index < commandLineArguments.Count; index++)
            {
                string argument = commandLineArguments[index];

                // Name
                if (CommandOptionInput.IsOption(argument))
                {
                    // Flush previous
                    if (!string.IsNullOrWhiteSpace(currentOptionAlias))
                    {
                        result.Add(new CommandOptionInput(currentOptionAlias, currentOptionValues));
                    }

                    currentOptionAlias = argument[2..];
                    currentOptionValues = new List<string>();
                }
                // Short name
                else if (CommandOptionInput.IsOptionAlias(argument))
                {
                    foreach (var alias in argument[1..])
                    {
                        // Flush previous
                        if (!string.IsNullOrWhiteSpace(currentOptionAlias))
                        {
                            result.Add(new CommandOptionInput(currentOptionAlias, currentOptionValues));
                        }

                        currentOptionAlias = alias.ToString();
                        currentOptionValues = new List<string>();
                    }
                }
                // Value
                else if (!string.IsNullOrWhiteSpace(currentOptionAlias))
                {
                    currentOptionValues.Add(argument);
                }
            }

            // Flush last option
            if (!string.IsNullOrWhiteSpace(currentOptionAlias))
            {
                result.Add(new CommandOptionInput(currentOptionAlias, currentOptionValues));
            }

            return result;
        }
    }
}
