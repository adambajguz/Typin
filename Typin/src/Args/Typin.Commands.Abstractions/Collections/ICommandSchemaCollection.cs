namespace Typin.Commands.Collections
{
    using System.Collections.Generic;
    using Typin.Commands.Schemas;
    using Typin.Schemas;
    using Typin.Schemas.Collections;

    /// <summary>
    /// Represents a collection of command schemas, where the key is <see cref="IAliasableSchema.Aliases"/>.
    /// </summary>
    public interface ICommandSchemaCollection : ISchemaCollection<IReadOnlyAliasCollection, string, ICommandSchema>
    {
        /// <summary>
        /// Finds all descendant commands of the parent command by name.
        /// </summary>
        IReadOnlyList<ICommandSchema> GetDescendantCommands(string? parentCommandName);

        /// <summary>
        /// Finds all child commands of the parent command by name.
        /// </summary>
        IReadOnlyList<ICommandSchema> GetChildCommands(string? parentCommandName);

        /// <summary>
        /// Checks if a name is a command or subcommand name part. This returns true even if there is no "cmd" command but "cmd sub" exists.
        /// </summary>
        bool IsCommandOrSubcommandPart(string? commandName);
    }
}
