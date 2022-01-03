namespace Typin.Models.Collections
{
    using System;
    using Typin.Models.Schemas;
    using Typin.Schemas.Collections;

    /// <summary>
    /// Default implementation of <see cref="IModelSchemaCollection"/>.
    /// </summary>
    public class ModelSchemaCollection : SchemaCollection<Type, IModelSchema>, IModelSchemaCollection
    {
        /// <summary>
        /// Initializes a new instance of <see cref="ModelSchemaCollection"/>.
        /// </summary>
        public ModelSchemaCollection() :
            base(schema => schema.Type)
        {

        }
    }
}
