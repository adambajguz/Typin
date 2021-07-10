namespace Typin.DynamicCommands
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Read-only dynamic argument collection.
    /// </summary>
    public interface IReadOnlyDynamicArgumentCollection : IEquatable<DynamicArgumentCollection?>, IEnumerable<KeyValuePair<string, object>>, IReadOnlyCollection<object>
    {
        /// <summary>
        /// Whether collection contains a property.
        /// </summary>
        /// <param name="propertyName">Property name.</param>
        /// <returns>Whether collection contains a property.</returns>
        /// <exception cref="ArgumentException">Throws when <paramref name="propertyName"/> is null or whitespace.</exception>
        bool Contains(string propertyName);

        /// <summary>
        /// Gets property value.
        /// </summary>
        /// <param name="propertyName">Property name.</param>
        /// <returns>Property value.</returns>
        /// <exception cref="ArgumentException">Throws when <paramref name="propertyName"/> is null or whitespace.</exception>
        object? GetValueOrDefault(string propertyName);

        /// <summary>
        /// Gets property value.
        /// </summary>
        /// <param name="propertyName">Property name.</param>
        /// <returns>Property value.</returns>
        /// <exception cref="ArgumentException">Throws when <paramref name="propertyName"/> is null or whitespace.</exception>
        T? GetValueOrDefault<T>(string propertyName);

        /// <summary>
        /// Gets property value.
        /// </summary>
        /// <param name="propertyName">Property name.</param>
        /// <returns>Property value.</returns>
        /// <exception cref="ArgumentException">Throws when <paramref name="propertyName"/> is null or whitespace.</exception>
        object GetValue(string propertyName);

        /// <summary>
        /// Gets property value.
        /// </summary>
        /// <param name="propertyName">Property name.</param>
        /// <returns>Property value.</returns>
        /// <exception cref="ArgumentException">Throws when <paramref name="propertyName"/> is null or whitespace.</exception>
        T GetValue<T>(string propertyName);
    }
}