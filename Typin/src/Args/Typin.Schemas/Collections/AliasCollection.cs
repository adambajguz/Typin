namespace Typin.Schemas.Collections
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Alias collection.
    /// </summary>
    public class AliasCollection : HashSet<string>, IAliasCollection
    {
        private static readonly StringComparer _comparer = StringComparer.Ordinal;

        /// <summary>
        /// Initializes a new instance of <see cref="AliasCollection"/>.
        /// </summary>
        public AliasCollection() :
            base(_comparer)
        {

        }

        /// <summary>
        /// Initializes a new instance of <see cref="AliasCollection"/>.
        /// </summary>
        public AliasCollection(IEnumerable<string> collection) :
            base(collection, _comparer)
        {

        }

        /// <summary>
        /// Initializes a new instance of <see cref="AliasCollection"/>.
        /// </summary>
        public AliasCollection(int capacity) :
            base(capacity, _comparer)
        {

        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            unchecked
            {
                int v = int.MinValue;

                foreach (string s in this)
                {
                    v += s.GetHashCode();
                }

                return v;
            }
        }

        /// <inheritdoc/>
        public override string? ToString()
        {
            return string.Join('|', this);
        }
    }
}
