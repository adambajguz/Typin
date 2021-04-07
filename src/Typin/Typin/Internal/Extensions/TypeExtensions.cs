namespace Typin.Internal.Extensions
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    internal static class TypeExtensions
    {
        public static Type? TryGetNullableUnderlyingType(this Type type)
        {
            return Nullable.GetUnderlyingType(type);
        }

        public static Type? TryGetEnumerableUnderlyingType(this Type type)
        {
            if (type.IsPrimitive)
                return null;

            if (type == typeof(IEnumerable))
                return typeof(object);

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                return type.GetGenericArguments().FirstOrDefault();

            return type.GetInterfaces()
                       .Select(TryGetEnumerableUnderlyingType)
                       .Where(t => t is not null)
                       .OrderByDescending(t => t != typeof(object)) // prioritize more specific types
                       .FirstOrDefault();
        }

        public static MethodInfo GetToStringMethod(this Type type)
        {
            // ToString() with no params always exists
            return type.GetMethod(nameof(ToString), Type.EmptyTypes)!;
        }

        public static bool IsToStringOverriden(this Type type)
        {
            return type.GetToStringMethod() != typeof(object).GetToStringMethod();
        }

        public static MethodInfo? TryGetStaticParseMethod(this Type type, bool withFormatProvider = false)
        {
            Type[] argumentTypes = withFormatProvider
                ? new[] { typeof(string), typeof(IFormatProvider) }
                : new[] { typeof(string) };

            return type.GetMethod("Parse",
                                  BindingFlags.Public | BindingFlags.Static,
                                  null,
                                  argumentTypes,
                                  null);
        }

        public static Array ToNonGenericArray<T>(this IEnumerable<T> source, Type elementType)
        {
            ICollection sourceAsCollection = source as ICollection ?? source.ToArray();

            Array array = Array.CreateInstance(elementType, sourceAsCollection.Count);
            sourceAsCollection.CopyTo(array, 0);

            return array;
        }
    }
}