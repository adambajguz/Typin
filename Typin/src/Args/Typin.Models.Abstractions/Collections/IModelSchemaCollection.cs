namespace Typin.Models.Collections
{
    using System;
    using Typin.Models.Schemas;
    using Typin.Schemas.Collections;

    /// <summary>
    /// Represents a collection of model schemas, where the key is <see cref="IModelSchema.Type"/>.
    /// </summary>
    public interface IModelSchemaCollection : ISchemaCollection<Type, IModelSchema>
    {

    }
}
