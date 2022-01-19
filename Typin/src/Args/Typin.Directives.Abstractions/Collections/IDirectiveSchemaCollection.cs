namespace Typin.Directives.Collections
{
    using Typin.Directives.Schemas;
    using Typin.Schemas;
    using Typin.Schemas.Collections;

    /// <summary>
    /// Represents a collection of directive schemas, where the key is <see cref="IAliasableSchema.Aliases"/>.
    /// </summary>
    public interface IDirectiveSchemaCollection : ISchemaCollection<IReadOnlyAliasCollection, string, IDirectiveSchema>
    {

    }
}
