namespace Typin.Commands.Collections
{
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
    }
}
