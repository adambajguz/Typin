namespace Typin.Models.Resolvers
{
    using System;
    using Typin.Models.Schemas;

    /// <summary>
    /// Model schema resolver.
    /// </summary>
    public interface IModelSchemaResolver : IAsyncResolver<IModelSchema>
    {
        /// <summary>
        /// Model type to resolve.
        /// </summary>
        Type ModelType { get; }
    }
}
