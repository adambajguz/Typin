namespace Typin.Commands.Collections
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Typin.Commands.Schemas;
    using Typin.Schemas.Collections;

    /// <summary>
    /// Default implementation for <see cref="ICommandSchemaCollection"/>.
    /// </summary>
    public class CommandSchemaCollection : SchemaCollection<string, ICommandSchema>, ICommandSchemaCollection
    {
        /// <summary>
        /// Initializes a new instance of <see cref="CommandSchemaCollection"/>.
        /// </summary>
        public CommandSchemaCollection() :
            base(schema => schema.Name)
        {

        }

        private static IEnumerable<ICommandSchema> GetDescendantCommands(IEnumerable<ICommandSchema> potentialParentCommands, string? parentCommandName)
        {
            parentCommandName = parentCommandName?.Trim() ?? string.Empty;

            return potentialParentCommands.Where(c => string.IsNullOrWhiteSpace(parentCommandName) ||
                                                 c.Name!.StartsWith(parentCommandName + ' ', StringComparison.Ordinal));
        }

        /// <inheritdoc/>
        public IReadOnlyList<ICommandSchema> GetDescendantCommands(string? parentCommandName)
        {
            return GetDescendantCommands(Data.Values, parentCommandName).ToArray();
        }

        /// <summary>
        /// Finds all child commands of the parrent command by name.
        /// </summary>
        public IReadOnlyList<ICommandSchema> GetChildCommands(string? parentCommandName)
        {
            IEnumerable<ICommandSchema> descendants = GetDescendantCommands(Data.Values, parentCommandName);

            // Filter out descendants of descendants, leave only children
            List<ICommandSchema> result = new(descendants);

            foreach (var descendant in descendants)
            {
                var descendantDescendants = GetDescendantCommands(descendants, descendant.Name).ToHashSet();
                result.RemoveAll(t => descendantDescendants.Contains(t));
            }

            return result;
        }

        /// <inheritdoc/>
        public bool IsCommandOrSubcommandPart(string? commandName)
        {
            if (string.IsNullOrWhiteSpace(commandName))
            {
                return false;
            }

            if (Data.ContainsKey(commandName))
            {
                return true;
            }

            commandName = string.Concat(commandName.Trim(), " ");

            return Data.Keys.Where(x => x.StartsWith(commandName)).Any();
        }
    }
}
