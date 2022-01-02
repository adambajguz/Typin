namespace Typin.Schemas
{
    using Typin.Schemas.Collections;

    /// <summary>
    /// Represents a schema.
    /// </summary>
    public interface ISchema
    {
        /// <summary>
        /// A collection of schema extensions.
        /// </summary>
        IExtensionsCollection Extensions { get; }
    }
}