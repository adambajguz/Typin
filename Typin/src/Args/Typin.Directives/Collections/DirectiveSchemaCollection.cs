namespace Typin.Directives.Collections
{
    using Typin.Directives.Schemas;
    using Typin.Schemas.Collections;
    using Typin.Schemas.Comparers;

    /// <summary>
    /// Default implementation of <see cref="IDirectiveSchemaCollection"/>.
    /// </summary>
    public class DirectiveSchemaCollection : SchemaCollection<IReadOnlyAliasCollection, IDirectiveSchema>, IDirectiveSchemaCollection
    {
        /// <summary>
        /// Initializes a new instance of <see cref="DirectiveSchemaCollection"/>.
        /// </summary>
        public DirectiveSchemaCollection() :
            base(schema => schema.Aliases, DefaultAliasCollectionComparer.Instance)
        {

        }

        /// <inheritdoc />
        public IDirectiveSchema? this[string key]
        {
            get => this[new AliasCollection { key }];
            set => this[new AliasCollection { key }] = value;
        }

        /// <inheritdoc />
        public IDirectiveSchema? Get(string key)
        {
            return Get(new AliasCollection
            {
                key
            });
        }

        /// <inheritdoc />
        public bool Remove(string key)
        {
            return Remove(new AliasCollection
            {
                key
            });
        }
    }
}
