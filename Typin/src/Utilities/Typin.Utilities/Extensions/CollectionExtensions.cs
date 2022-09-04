namespace Typin.Utilities.Extensions
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// <see cref="ICollection{T}"/> extensions.
    /// </summary>
    public static class CollectionExtensions
    {
        /// <summary>
        /// Removes <paramref name="items"/> from <paramref name="source"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="items"></param>
        public static void RemoveRange<T>(this ICollection<T> source, IEnumerable<T> items)
        {
            foreach (T item in items)
            {
                source.Remove(item);
            }
        }

        /// <summary>
        /// Converts enumerable to non generic array.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="elementType"></param>
        /// <returns></returns>
        public static Array ToNonGenericArray<T>(this IEnumerable<T> source, Type elementType)
        {
            ICollection sourceAsCollection = source as ICollection ?? source.ToArray();

            var array = Array.CreateInstance(elementType, sourceAsCollection.Count);
            sourceAsCollection.CopyTo(array, 0);

            return array;
        }
    }
}