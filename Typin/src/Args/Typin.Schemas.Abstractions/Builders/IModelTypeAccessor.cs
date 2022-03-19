namespace Typin.Schemas.Builders
{
    using System;

    /// <summary>
    /// Represents a class with model type accessor property.
    /// </summary>
    public interface IModelTypeAccessor
    {
        /// <summary>
        /// Related model type.
        /// </summary>
        Type ModelType { get; }
    }
}
