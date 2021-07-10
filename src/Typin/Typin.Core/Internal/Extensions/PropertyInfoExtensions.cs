namespace Typin.Internal.Extensions
{
    using System;
    using System.Reflection;

    internal static class PropertyInfoExtensions
    {
        public static Type? TryGetEnumerableArgumentUnderlyingType(this PropertyInfo? property)
        {
            return property is not null && property.PropertyType != typeof(string)
                       ? property.PropertyType.TryGetEnumerableUnderlyingType()
                       : null;
        }

        public static Type? TryGetEnumerableArgumentUnderlyingType(this Type? type)
        {
            return type is not null && type != typeof(string)
                       ? type.TryGetEnumerableUnderlyingType()
                       : null;
        }
    }
}