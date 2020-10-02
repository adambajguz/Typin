namespace Typin.OptionFallback
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Option fallback value provider.
    /// </summary>
    public interface IOptionFallbackProvider : IEnumerable<KeyValuePair<string, string>>, IEnumerable<KeyValuePair<(string, Type), string>>
    {
        /// <summary>
        /// Gets an enumerable collection that contains the keys that can be used as a fallback.
        /// </summary>
        IEnumerable<string> Keys { get; }

        /// <summary>
        /// Gets an enumerable collection that contains the values.
        /// </summary>
        IEnumerable<string> Values { get; }

        /// <summary>
        /// Gets the number of fallback values in the collection.
        /// </summary>
        public int Count { get; }

        /// <summary>
        /// Gets the element that has the specified key in the read-only dictionary.
        /// </summary>
        /// <param name="key">The key to locate.</param>
        /// <returns>The element that has the specified key in the read-only dictionary.</returns>
        /// <exception cref="KeyNotFoundException">The property is retrieved and key is not found.</exception>
        string this[string key] { get; }

        /// <summary>
        /// Gets the element that has the specified key in the read-only dictionary.
        /// </summary>
        /// <param name="key">The key to locate.</param>
        /// <param name="targetType">The target type.</param>
        /// <returns>The element that has the specified key in the read-only dictionary.</returns>
        /// <exception cref="KeyNotFoundException">The property is retrieved and key is not found.</exception>
        string this[string key, Type targetType] { get; }

        /// <summary>
        /// Gets the raw value that is associated with the specified key.
        /// </summary>
        /// <param name="key">The key to locate.</param>
        /// <param name="value">When this method returns, the value associated with the specified key, if the key is found; otherwise, the default value for the type of the value parameter. This parameter is passed uninitialized.</param>
        /// <returns>true if contains an element that has the specified key; otherwise, false.</returns>
        bool TryGetValue(string key, out string value);

        /// <summary>
        /// Gets the value that is associated with the specified key. This method is called by Typin to to get fallback value.
        /// </summary>
        /// <param name="key">The key to locate.</param>
        /// <param name="targetType">The target type.</param>
        /// <param name="value">When this method returns, the value associated with the specified key, if the key is found; otherwise, the default value for the type of the value parameter. This parameter is passed uninitialized.</param>
        /// <returns>true if contains an element that has the specified key; otherwise, false.</returns>
        bool TryGetValue(string key, Type targetType, out string value);
    }
}
