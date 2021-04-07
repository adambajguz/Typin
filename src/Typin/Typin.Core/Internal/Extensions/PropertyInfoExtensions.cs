namespace Typin.Internal.Extensions
{
    using System;
    using System.Linq;
    using System.Reflection;

    internal static class PropertyInfoExtensions
    {
        public static Type? TryGetEnumerableArgumentUnderlyingType(this PropertyInfo? property)
        {
            return property is not null && property.PropertyType != typeof(string)
                       ? property.PropertyType.TryGetEnumerableUnderlyingType()
                       : null;
        }
    }
}