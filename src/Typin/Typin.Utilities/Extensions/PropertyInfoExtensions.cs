namespace Typin.Utilities.Extensions
{
    using System;
    using System.Reflection;

    /// <summary>
    /// <see cref="PropertyInfo"/> extensions.
    /// </summary>
    public static class PropertyInfoExtensions
    {
        /// <summary>
        /// Tries to get enumerable underlying type.
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public static Type? TryGetEnumerableArgumentUnderlyingType(this PropertyInfo? property)
        {
            return property is not null && property.PropertyType != typeof(string)
                       ? property.PropertyType.TryGetEnumerableUnderlyingType()
                       : null;
        }

        /// <summary>
        /// Tries to get enumerable underlying type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Type? TryGetEnumerableArgumentUnderlyingType(this Type? type)
        {
            return type is not null && type != typeof(string)
                       ? type.TryGetEnumerableUnderlyingType()
                       : null;
        }
    }
}