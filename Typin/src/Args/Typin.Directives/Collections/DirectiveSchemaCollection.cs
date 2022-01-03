namespace Typin.Directives.Collections
{
    using Typin.Directives.Schemas;
    using Typin.Schemas.Collections;

    /// <summary>
    /// Default implementation of <see cref="IDirectiveSchemaCollection"/>.
    /// </summary>
    public class DirectiveSchemaCollection : SchemaCollection<string, IDirectiveSchema>, IDirectiveSchemaCollection
    {
        /// <summary>
        /// Initializes a new instance of <see cref="DirectiveSchemaCollection"/>.
        /// </summary>
        public DirectiveSchemaCollection() :
            base(schema => schema.Name)
        {

        }
    }
}
