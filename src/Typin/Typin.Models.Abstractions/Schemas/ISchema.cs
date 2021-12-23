namespace Typin.Models.Schemas
{
    using Typin.Models.Collections;

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