namespace Typin.Schemas.Comparers
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Typin.Schemas.Collections;

    /// <summary>
    /// Default <see cref="IAliasCollection"/> comparer.
    /// </summary>
    public sealed class DefaultAliasCollectionComparer : IEqualityComparer<IReadOnlyAliasCollection>
    {
        /// <summary>
        /// Comparer instance.
        /// </summary>
        public static DefaultAliasCollectionComparer Instance { get; } = new DefaultAliasCollectionComparer();

        /// <summary>
        /// Initializes a new instance of <see cref="DefaultAliasCollectionComparer"/>.
        /// </summary>
        public DefaultAliasCollectionComparer()
        {

        }

        /// <inheritdoc/>
        public bool Equals(IReadOnlyAliasCollection? x, IReadOnlyAliasCollection? y)
        {
            if (object.ReferenceEquals(x, y))
            {
                return true;
            }

            if (x is null || y is null)
            {
                return false;
            }

            return x.IsSubsetOf(y);
        }

        /// <inheritdoc/>
        public int GetHashCode([DisallowNull] IReadOnlyAliasCollection obj)
        {
            return obj.GetHashCode();
        }
    }
}
