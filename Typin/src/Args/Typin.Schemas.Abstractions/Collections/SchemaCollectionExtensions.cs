namespace Typin.Schemas.Collections
{
    using System.Collections.Generic;

    /// <summary>
    /// Schema collection extensions.
    /// </summary>
    public static class SchemaCollectionExtensions
    {
        /// <summary>
        /// Get all aliases.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static IEnumerable<string> GetAliases<T>(this IEnumerable<KeyValuePair<IReadOnlyAliasCollection, T>> collection)
            where T : class
        {
            foreach (KeyValuePair<IReadOnlyAliasCollection, T> pair in collection)
            {
                foreach (string alias in pair.Key)
                {
                    yield return alias;
                }
            }
        }
    }
}
