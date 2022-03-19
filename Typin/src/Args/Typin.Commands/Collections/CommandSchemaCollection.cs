namespace Typin.Commands.Collections
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Typin.Commands.Schemas;
    using Typin.Schemas.Collections;
    using Typin.Schemas.Comparers;

    /// <summary>
    /// Default implementation of <see cref="ICommandSchemaCollection"/>.
    /// </summary>
    public class CommandSchemaCollection : SchemaCollection<IReadOnlyAliasCollection, ICommandSchema>, ICommandSchemaCollection
    {
        /// <summary>
        /// Initializes a new instance of <see cref="CommandSchemaCollection"/>.
        /// </summary>
        public CommandSchemaCollection() :
            base(schema => schema.Aliases, DefaultAliasCollectionComparer.Instance)
        {

        }

        /// <inheritdoc />
        public ICommandSchema? this[string key]
        {
            get => this[new AliasCollection { key }];
            set => this[new AliasCollection { key }] = value;
        }

        /// <inheritdoc />
        public ICommandSchema? Get(string key)
        {
            return Get(new AliasCollection
            {
                key
            });
        }

        /// <inheritdoc />
        public bool Remove(string key)
        {
            return Remove(new AliasCollection
            {
                key
            });
        }

        private static IEnumerable<ICommandSchema> GetDescendantCommands(IEnumerable<ICommandSchema> potentialParentCommands, string? parentCommandName)
        {
            parentCommandName = parentCommandName?.Trim() ?? string.Empty;
            string v = parentCommandName + ' ';

            return potentialParentCommands.Where(c => string.IsNullOrWhiteSpace(parentCommandName) ||
                                                 c.Aliases!.Any(y => y.StartsWith(v, StringComparison.Ordinal)));
        }

        /// <inheritdoc/>
        public IReadOnlyList<ICommandSchema> GetDescendantCommands(string? parentCommandName)
        {
            return GetDescendantCommands(Data.Values, parentCommandName).ToArray();
        }

        /// <summary>
        /// Finds all child commands of the parent command by name.
        /// </summary>
        public IReadOnlyList<ICommandSchema> GetChildCommands(string? parentCommandName)
        {
            IEnumerable<ICommandSchema> descendants = GetDescendantCommands(Data.Values, parentCommandName);

            // Filter out descendants of descendants, leave only children
            List<ICommandSchema> result = new(descendants);

            foreach (ICommandSchema descendant in descendants)
            {
                foreach (string alias in descendant.Aliases)
                {
                    var descendantDescendants = GetDescendantCommands(descendants, alias).ToHashSet();
                    result.RemoveAll(t => descendantDescendants.Contains(t));
                }
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

            if (Data.ContainsKey(new AliasCollection { commandName }))
            {
                return true;
            }

            commandName = string.Concat(commandName.Trim(), " ");

            return Data.Keys.Where(x => x.Any(y => y.StartsWith(commandName))).Any();
        }
    }
}
