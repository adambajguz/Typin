namespace Typin.OptionFallback
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Environment variable fallback value provider.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class EnvironmentVariableFallbackProvider : IOptionFallbackProvider
    {
        private readonly IReadOnlyDictionary<string, string> _environmentVariables;

        /// <summary>
        /// Initializes an instance of <see cref="EnvironmentVariableFallbackProvider"/>.
        /// </summary>
        public EnvironmentVariableFallbackProvider(ICliContext cliContext)
        {
            _environmentVariables = cliContext.EnvironmentVariables;
        }

        /// <inheritdoc/>
        public IEnumerable<string> Keys => _environmentVariables.Keys;

        /// <inheritdoc/>
        public IEnumerable<string> Values => _environmentVariables.Values;

        /// <inheritdoc/>
        public int Count => _environmentVariables.Count;

        /// <inheritdoc/>
        public string this[string key] => _environmentVariables[key];

        /// <inheritdoc/>
        public string this[string key, Type targetType] => _environmentVariables[key];

        /// <inheritdoc/>
        public bool TryGetValue(string key, out string value)
        {
            return _environmentVariables.TryGetValue(key, out value);
        }

        /// <inheritdoc/>
        public bool TryGetValue(string key, Type targetType, out string value)
        {
            return _environmentVariables.TryGetValue(key, out value);
        }

        /// <inheritdoc/>
        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            return _environmentVariables.GetEnumerator();
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return (_environmentVariables as IEnumerable).GetEnumerator();
        }

        /// <inheritdoc/>
        IEnumerator<KeyValuePair<(string, Type), string>> IEnumerable<KeyValuePair<(string, Type), string>>.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
