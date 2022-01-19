namespace Typin.Schemas
{
    using Typin.Schemas.Collections;

    /// <summary>
    /// Represents an aliasable schema.
    /// </summary>
    public interface IAliasableSchema : ISchema
    {
        /// <summary>
        /// A collection of schema names aka. aliases.
        /// </summary>
        IReadOnlyAliasCollection Aliases { get; }
    }
}