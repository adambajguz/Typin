namespace Typin.Commands.Collections
{
    using Typin.Commands.Schemas;
    using Typin.Schemas.Collections;

    /// <summary>
    /// Represents a collection of command schemas, where the key is <see cref="ICommandSchema.Name"/>.
    /// </summary>
    public interface ICommandSchemaCollection : ISchemaCollection<string, ICommandSchema>
    {

    }
}
