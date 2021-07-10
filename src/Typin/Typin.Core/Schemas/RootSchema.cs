namespace Typin.Schemas
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Stores all schemas of commands and directives in the application.
    /// </summary>
    public class RootSchema
    {
        private readonly Dictionary<string, CommandSchema> _commands;
        private HashSet<string>? _directiveNamesHashSet;
        private HashSet<string>? _commandNamesHashSet;
        private readonly Lazy<IReadOnlyDictionary<Type, BaseCommandSchema>> _dynamicCommands;

        /// <summary>
        /// List of defined directives in the application.
        /// </summary>
        public IReadOnlyDictionary<string, DirectiveSchema> Directives { get; }

        /// <summary>
        /// List of defined commands in the application.
        /// </summary>
        public IReadOnlyDictionary<string, CommandSchema> Commands => _commands;


        /// <summary>
        /// Default command (null if no default command).
        /// </summary>
        public CommandSchema? DefaultCommand { get; }

        /// <summary>
        /// Dynamic commands.
        /// </summary>
        public IEnumerable<BaseCommandSchema> DynamicCommands => _dynamicCommands.Value.Values;

        /// <summary>
        /// Initializes an instance of <see cref="RootSchema"/>.
        /// </summary>
        public RootSchema(IReadOnlyDictionary<string, DirectiveSchema> directives,
                          Lazy<IReadOnlyDictionary<Type, BaseCommandSchema>> dynamicCommands,
                          Dictionary<string, CommandSchema> commands,
                          CommandSchema? defaultCommand)
        {
            Directives = directives;
            _commands = commands;
            DefaultCommand = defaultCommand;
            _dynamicCommands = dynamicCommands;
        }

        /// <summary>
        /// Tries to add a dynamic command.
        /// </summary>
        /// <param name="commandSchema"></param>
        /// <returns>Whether dynamic command was added.</returns>
        public bool TryAddDynamicCommand(CommandSchema commandSchema)
        {
            if (commandSchema.IsDynamic && !commandSchema.IsDefault)
            {
                bool added = _commands.TryAdd(commandSchema.Name!, commandSchema);

                if (added)
                {
                    _commandNamesHashSet = null;
                }

                return added;
            }

            return false;
        }

        /// <summary>
        /// Tries to add all dynamic commands.
        /// </summary>
        /// <param name="commandSchemas"></param>
        /// <returns>Whether all dynamic commands were added.</returns>
        public bool TryAddDynamicCommandRange(IEnumerable<CommandSchema> commandSchemas)
        {
            bool allAdded = true;
            foreach (CommandSchema schema in commandSchemas)
            {
                if (schema.IsDynamic && !schema.IsDefault)
                {
                    allAdded &= _commands.TryAdd(schema.Name!, schema);
                }
                else
                {
                    allAdded = false;
                }
            }
            _commandNamesHashSet = null;

            return allAdded;
        }

        /// <summary>
        /// Tries to remove a dynamic command.
        /// </summary>
        /// <param name="name">Command name to remove.</param>
        /// <returns>Whether dynamic command was added.</returns>
        public bool TryRemoveDynamicCommand(string name)
        {
            if (_commands.TryGetValue(name, out CommandSchema? commandSchema) && commandSchema.IsDynamic)
            {
                _commands.Remove(name);
                _commandNamesHashSet = null;

                return true;
            }

            return false;
        }

        /// <summary>
        /// Tries to remove a dynamic commands.
        /// </summary>
        /// <param name="names">Command names to remove.</param>
        /// <returns>Whether all dynamic commands were removed.</returns>
        public bool TryRemoveDynamicCommands(IEnumerable<string> names)
        {
            bool allRemoved = true;
            foreach (string name in names)
            {
                if (_commands.TryGetValue(name, out CommandSchema? commandSchema) && commandSchema.IsDynamic)
                {
                    _commands.Remove(name);
                }
                else
                {
                    allRemoved = false;
                }
            }

            _commandNamesHashSet = null;

            return allRemoved;
        }

        /// <summary>
        /// Returns collection of commands names.
        /// </summary>
        public ISet<string> GetCommandNames()
        {
            return (_commandNamesHashSet ??= _commands.Keys.ToHashSet(StringComparer.Ordinal));
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
        public bool IsCommandOrSubcommandPart(string? commandName)
        {
            if (string.IsNullOrWhiteSpace(commandName))
            {
                return false;
            }

            if (_commands.ContainsKey(commandName))
            {
                return true;
            }

            commandName = string.Concat(commandName.Trim(), " ");

            return _commands.Keys.Where(x => x.StartsWith(commandName)).Any();
        }

        /// <summary>
        /// Finds command schema by name.
        /// </summary>
        public CommandSchema? TryFindCommand(string? commandName)
        {
            if (string.IsNullOrWhiteSpace(commandName))
            {
                return DefaultCommand;
            }

            return _commands.GetValueOrDefault(commandName);
        }

        /// <summary>
        /// Finds dynamic command base schema by type.
        /// </summary>
        public BaseCommandSchema? TryFindDynamicCommandBase(Type type)
        {
            return _dynamicCommands.Value.GetValueOrDefault(type);
        }

        /// <summary>
        /// Finds command schema by name.
        /// </summary>
        public DirectiveSchema? TryFindDirective(string directiveName)
        {
            if (string.IsNullOrWhiteSpace(directiveName))
            {
                return null;
            }

            return Directives.GetValueOrDefault(directiveName);
        }

        private static IEnumerable<CommandSchema> GetDescendantCommands(IEnumerable<CommandSchema> potentialParentCommands, string? parentCommandName)
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
            List<CommandSchema> result = new(descendants);

            foreach (var descendant in descendants)
            {
                var descendantDescendants = GetDescendantCommands(descendants, descendant.Name).ToHashSet();
                result.RemoveAll(t => descendantDescendants.Contains(t));
            }

            return result;
        }
    }
}