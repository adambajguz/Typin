namespace Typin.DynamicCommands
{
    using System;

    /// <summary>
    /// Dynamic argument collection.
    /// </summary>
    public interface IDynamicArgumentCollection : IReadOnlyDynamicArgumentCollection
    {
        /// <summary>
        /// Sets a property value.
        /// </summary>
        /// <param name="propertyName">Property name.</param>
        /// <param name="value">Value to set.</param>
        /// <exception cref="ArgumentException">Throws when <paramref name="propertyName"/> is null or whitespace.</exception>
        void SetValue(string propertyName, object? value);
    }
}