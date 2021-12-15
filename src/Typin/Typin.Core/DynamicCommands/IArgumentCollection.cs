namespace Typin.DynamicCommands
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Argument collection.
    /// </summary>
    public interface IArgumentCollection : IReadOnlyArgumentCollection
    {
        /// <summary>
        /// Gets or sets argument value.
        /// Setting a null value will remove the property.
        /// </summary>
        /// <param name="propertyName">Property name.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Throws when <paramref name="propertyName"/> is null or whitespace.</exception>
        /// <exception cref="KeyNotFoundException">Throws when <paramref name="propertyName"/> was not found.</exception>
        new InputValue? this[string propertyName] { get; set; }

        /// <summary>
        /// Sets argument value.
        /// A null value will remove the property.
        /// </summary>
        /// <param name="propertyName">Property name.</param>
        /// <param name="value">Value to set.</param>
        /// <exception cref="ArgumentException">Throws when <paramref name="propertyName"/> is null or whitespace.</exception>
        void Set(string propertyName, InputValue? value);

        /// <summary>
        /// Sets argument value.
        /// </summary>
        /// <param name="propertyName">Property name.</param>
        /// <param name="value">Value to set.</param>
        /// <exception cref="ArgumentException">Throws when <paramref name="propertyName"/> is null or whitespace.</exception>
        void Set(string propertyName, object? value);

        /// <summary>
        /// Removes a property by name.
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns>True when property was removed, false when not.</returns>
        bool Remove(string propertyName);
    }
}