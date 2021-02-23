namespace Typin
{
    using Typin.Schemas;

    /// <summary>
    /// Service that can be used to access root schema.
    /// </summary>
    public interface IRootSchemaAccessor
    {
        /// <summary>
        /// Root schema.
        /// </summary>
        RootSchema RootSchema { get; }
    }
}
