namespace Typin.DynamicCommands
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Read-only argument collection.
    /// </summary>
    public interface IReadOnlyArgumentCollection : IReadOnlyCollection<KeyValuePair<string, InputValue>>
    {
        /// <summary>
        /// Gets or sets argument value.
        /// </summary>
        /// <param name="propertyName">Property name.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Throws when <paramref name="propertyName"/> is null or whitespace.</exception>
        /// <exception cref="KeyNotFoundException">Throws when <paramref name="propertyName"/> was not found.</exception>
        InputValue this[string propertyName] { get; }

        /// <summary>
        /// Whether collection contains an argument.
        /// </summary>
        /// <param name="propertyName">Property name.</param>
        /// <returns>Whether collection contains a property.</returns>
        /// <exception cref="ArgumentException">Throws when <paramref name="propertyName"/> is null or whitespace.</exception>
        bool Contains(string propertyName);

        /// <summary>
        /// Gets an arguemnt.
        /// </summary>
        /// <param name="propertyName">Property name.</param>
        /// <returns>Property value.</returns>
        /// <exception cref="ArgumentException">Throws when <paramref name="propertyName"/> is null or whitespace.</exception>
        /// <exception cref="KeyNotFoundException">Throws when <paramref name="propertyName"/> was not found.</exception>
        InputValue Get(string propertyName);

        /// <summary>
        /// Gets an argument.
        /// </summary>
        /// <param name="propertyName">Property name.</param>
        /// <returns>Property value.</returns>
        /// <exception cref="ArgumentException">Throws when <paramref name="propertyName"/> is null or whitespace.</exception>
        InputValue? TryGet(string propertyName);
    }
}