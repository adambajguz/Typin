namespace Typin.Schemas
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    /// <summary>
    /// Stores all schemas of commands and directives in the application.
    /// </summary>
    public class RootSchema
    {
        /// <summary>
        /// List of defined directives in the application.
        /// </summary>
        public IReadOnlyDictionary<string, DirectiveSchema> Directives { get; }

        private HashSet<string>? _directiveNamesHashSet;

        /// <summary>
        /// List of defined commands in the application.
        /// </summary>
        public IReadOnlyDictionary<string, CommandSchema> Commands { get; }

        private HashSet<string>? _commandNamesHashSet;

        /// <summary>
        /// Default command (null if no default command).
        /// </summary>
        public CommandSchema? DefaultCommand { get; }

        /// <summary>
        /// Initializes an instance of <see cref="RootSchema"/>.
        /// </summary>
        public RootSchema(IReadOnlyDictionary<string, DirectiveSchema> directives,
                          IReadOnlyDictionary<string, CommandSchema> commands,
                          CommandSchema? defaultCommand)
        {
            Directives = directives;
            Commands = commands;
            DefaultCommand = defaultCommand;
        }

        /// <summary>
        /// Returns collection of commands names.
        /// </summary>
        public ISet<string> GetCommandNames()
        {
            return (_commandNamesHashSet ??= Commands.Keys.ToHashSet(StringComparer.Ordinal));
        }

        /// <summary>
        /// Returns collection of directives names.
        /// </summary>
        public ISet<string> GetDirectivesNames()
        {
            return (_directiveNamesHashSet ??= Directives.Keys.ToHashSet(StringComparer.Ordinal));
        }

        /// <summary>
        /// Checks if a name is a command or subcommand name part. This returns true even if there is no "cmd" command but "cmd sub" exists.
        /// </summary>
        public bool IsCommandOrSubcommandPart(string? commandName, bool hasDefaultDirective = false)
        {
            if (hasDefaultDirective || string.IsNullOrWhiteSpace(commandName))
                return true;

            if (Commands.ContainsKey(commandName))
                return true;

            commandName = string.Concat(commandName.Trim(), " ");

            return Commands.Keys.Where(x => x.StartsWith(commandName)).Any();
        }

        /// <summary>
        /// Finds command schema by name.
        /// </summary>
        public CommandSchema? TryFindCommand(string? commandName, bool hasDefaultDirective = false)
        {
            if (hasDefaultDirective || string.IsNullOrWhiteSpace(commandName))
                return DefaultCommand;

            Commands.TryGetValue(commandName, out CommandSchema? value);

            return value;
        }

        /// <summary>
        /// Finds command schema by name.
        /// </summary>
        public DirectiveSchema? TryFindDirective(string directiveName)
        {
            if (string.IsNullOrWhiteSpace(directiveName))
                return null;

            Directives.TryGetValue(directiveName, out DirectiveSchema? value);

            return value;
        }

        [SuppressMessage("Performance", "CA1822:Mark members as static")]
        private IEnumerable<CommandSchema> GetDescendantCommands(IEnumerable<CommandSchema> potentialParentCommands, string? parentCommandName)
        {
            return potentialParentCommands.Where(c => string.IsNullOrWhiteSpace(parentCommandName) ||
                                                 c.Name!.StartsWith(parentCommandName + ' ', StringComparison.Ordinal));
        }

        /// <summary>
        /// Finds all descendant commands of the parrent command by name.
        /// </summary>
        public IReadOnlyList<CommandSchema> GetDescendantCommands(string? parentCommandName)
        {
            return GetDescendantCommands(Commands.Values, parentCommandName).ToArray();
        }

        /// <summary>
        /// Finds all child commands of the parrent command by name.
        /// </summary>
        public IReadOnlyList<CommandSchema> GetChildCommands(string? parentCommandName)
        {
            IEnumerable<CommandSchema> descendants = GetDescendantCommands(Commands.Values, parentCommandName);

            // Filter out descendants of descendants, leave only children
            var result = new List<CommandSchema>(descendants);

            foreach (var descendant in descendants)
            {
                var descendantDescendants = GetDescendantCommands(descendants, descendant.Name).ToHashSet();
                result.RemoveAll(t => descendantDescendants.Contains(t));
            }

            return result;
        }
    }
}