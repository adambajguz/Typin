namespace Typin.Directives.Collections
{
    using Typin.Directives.Schemas;
    using Typin.Schemas.Collections;

    /// <summary>
    /// Represents a collection of Directive schemas, where the key is <see cref="IDirectiveSchema.Name"/>.
    /// </summary>
    public interface IDirectiveSchemaCollection : ISchemaCollection<string, IDirectiveSchema>
    {

    }
}
