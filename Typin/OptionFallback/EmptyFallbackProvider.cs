namespace Typin.OptionFallback
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Empty variable fallback value provider.
    /// </summary>
    public class EmptyFallbackProvider : IOptionFallbackProvider
    {
        /// <summary>
        /// Initializes an instance of <see cref="EmptyFallbackProvider"/>.
        /// </summary>
        public EmptyFallbackProvider()
        {

        }

        /// <inheritdoc/>
        public bool IsDynamic => true;

        /// <inheritdoc/>
        public IEnumerable<string> Keys => Enumerable.Empty<string>();

        /// <inheritdoc/>
        public IEnumerable<string> Values => Enumerable.Empty<string>();

        /// <inheritdoc/>
        public int Count => 0;

        /// <inheritdoc/>
        public string this[string key] => throw new KeyNotFoundException();

        /// <inheritdoc/>
        public string this[string key, Type? targetType] => throw new KeyNotFoundException();

        /// <inheritdoc/>
        public bool TryGetValue(string key, out string value)
        {
            value = null!;
            return false;
        }

        /// <inheritdoc/>
        public bool TryGetValue(string key, Type? targetType, out string value)
        {
            value = null!;
            return false;
        }

        /// <inheritdoc/>
        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            return Enumerable.Empty<KeyValuePair<string, string>>().GetEnumerator();
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return (Enumerable.Empty<KeyValuePair<string, string>>() as IEnumerable).GetEnumerator();
        }

        /// <inheritdoc/>
        IEnumerator<KeyValuePair<(string, Type?), string>> IEnumerable<KeyValuePair<(string, Type?), string>>.GetEnumerator()
        {
            return Enumerable.Empty<KeyValuePair<(string, Type?), string>>().GetEnumerator();
        }
    }
}
