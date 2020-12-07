namespace Typin.Internal.Input
{
    using System;
    using System.Collections.Generic;
    using Typin.Directives;
    using Typin.Input;
    using Typin.Internal.Extensions;

    /// <summary>
    /// Resolves an instance of <see cref="CommandInput"/>.
    /// </summary>
    internal class CommandInputResolver
    {
        /// <summary>
        /// Resolves <see cref="CommandInput"/>.
        /// </summary>
        public static CommandInput Parse(IReadOnlyList<string> commandLineArguments,
                                         ISet<string> availableCommandNamesSet)
        {
            int index = 0;

            IReadOnlyList<DirectiveInput> directives = ParseDirectives(
                commandLineArguments,
                ref index,
                out bool isDefaultDirectiveSpecified
            );

            string? commandName = ParseCommandName(
                commandLineArguments,
                availableCommandNamesSet,
                isDefaultDirectiveSpecified,
                ref index
            );

            IReadOnlyList<CommandParameterInput> parameters = ParseParameters(
                commandLineArguments,
                ref index
            );

            IReadOnlyList<CommandOptionInput> options = ParseOptions(
                commandLineArguments,
                ref index
            );

            return new CommandInput(commandLineArguments, directives, commandName, parameters, options);
        }

        private static IReadOnlyList<DirectiveInput> ParseDirectives(IReadOnlyList<string> commandLineArguments,
                                                                     ref int index,
                                                                     out bool isDefaultDirectiveSpecified)
        {
            isDefaultDirectiveSpecified = false;

            var result = new List<DirectiveInput>();

            for (; index < commandLineArguments.Count; index++)
            {
                string argument = commandLineArguments[index];

                if (!argument.StartsWith('[') || !argument.EndsWith(']'))
                    break;

                string name = argument.Substring(startIndex: 1, length: argument.Length - 2);

                isDefaultDirectiveSpecified = name == BuiltInDirectives.Default;

                result.Add(new DirectiveInput(name));
            }

            return result;
        }

        private static string? ParseCommandName(IReadOnlyList<string> commandLineArguments,
                                                ISet<string> commandNames,
                                                bool isDefaultDirectiveSpecified,
                                                ref int index)
        {
            if (isDefaultDirectiveSpecified)
                return null;

            var buffer = new List<string>();

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
                index = lastIndex + 1;

            return commandName;
        }

        private static IReadOnlyList<CommandParameterInput> ParseParameters(IReadOnlyList<string> commandLineArguments,
                                                                            ref int index)
        {
            var result = new List<CommandParameterInput>();

            for (; index < commandLineArguments.Count; index++)
            {
                string argument = commandLineArguments[index];

                if (CommandOptionInput.IsOption(argument) | CommandOptionInput.IsOptionAlias(argument))
                    break;

                result.Add(new CommandParameterInput(argument));
            }

            return result;
        }

        private static IReadOnlyList<CommandOptionInput> ParseOptions(IReadOnlyList<string> commandLineArguments,
                                                                      ref int index)
        {
            var result = new List<CommandOptionInput>();

            string? currentOptionAlias = null;
            var currentOptionValues = new List<string>();

            for (; index < commandLineArguments.Count; index++)
            {
                string argument = commandLineArguments[index];

                // Name
                if (CommandOptionInput.IsOption(argument))
                {
                    // Flush previous
                    if (!string.IsNullOrWhiteSpace(currentOptionAlias))
                        result.Add(new CommandOptionInput(currentOptionAlias, currentOptionValues));

                    currentOptionAlias = argument.Substring(2); // TODO: --h is treated as -h because there is no distinction between - and -- in parser. What if we have --a and -a in command?
                    currentOptionValues = new List<string>();
                }
                // Short name
                else if (CommandOptionInput.IsOptionAlias(argument))
                    foreach (var alias in argument.Substring(1))
                    {
                        // Flush previous
                        if (!string.IsNullOrWhiteSpace(currentOptionAlias))
                            result.Add(new CommandOptionInput(currentOptionAlias, currentOptionValues));

                        currentOptionAlias = alias.ToString();
                        currentOptionValues = new List<string>();
                    }
                // Value
                else if (!string.IsNullOrWhiteSpace(currentOptionAlias))
                    currentOptionValues.Add(argument);
            }

            // Flush last option
            if (!string.IsNullOrWhiteSpace(currentOptionAlias))
                result.Add(new CommandOptionInput(currentOptionAlias, currentOptionValues));

            return result;
        }
    }
}
