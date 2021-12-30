namespace Typin.Commands.Collections
{
    using System.Collections.Generic;
    using Typin.Commands.Schemas;
    using Typin.Schemas.Collections;

    /// <summary>
    /// Represents a collection of command schemas, where the key is <see cref="ICommandSchema.Name"/>.
    /// </summary>
    public interface ICommandSchemaCollection : ISchemaCollection<string, ICommandSchema>
    {
        /// <summary>
        /// Finds all descendant commands of the parrent command by name.
        /// </summary>
        IReadOnlyList<ICommandSchema> GetDescendantCommands(string? parentCommandName);

        /// <summary>
        /// Finds all child commands of the parrent command by name.
        /// </summary>
        IReadOnlyList<ICommandSchema> GetChildCommands(string? parentCommandName);

        /// <summary>
        /// Checks if a name is a command or subcommand name part. This returns true even if there is no "cmd" command but "cmd sub" exists.
        /// </summary>
        bool IsCommandOrSubcommandPart(string? commandName);
    }
}
