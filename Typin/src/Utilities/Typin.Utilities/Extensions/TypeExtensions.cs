namespace Typin.Utilities.Extensions
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// <see cref="Type"/> extensions.
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        /// Checks whether type implements an interface.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="interfaceType"></param>
        /// <returns></returns>
        public static bool Implements(this Type type, Type interfaceType)
        {
            return type.GetInterfaces().Contains(interfaceType);
        }

        /// <summary>
        /// Tries to get enumerable underlying type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Type? TryGetNullableUnderlyingType(this Type type)
        {
            return Nullable.GetUnderlyingType(type);
        }

        /// <summary>
        /// Tries to get enumerable underlying type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Type? TryGetEnumerableUnderlyingType(this Type type)
        {
            if (type.IsPrimitive)
            {
                return null;
            }

            if (type == typeof(IEnumerable))
            {
                return typeof(object);
            }

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable<>))
            {
                return type.GetGenericArguments().FirstOrDefault();
            }

            return type.GetInterfaces()
                       .Select(TryGetEnumerableUnderlyingType)
                       .Where(t => t != null)
                       .OrderByDescending(t => t != typeof(object)) // prioritize more specific types
                       .FirstOrDefault();
        }

        /// <summary>
        /// Gets to string method.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static MethodInfo GetToStringMethod(this Type type)
        {
            // ToString() with no params always exists
            return type.GetMethod(nameof(ToString), Type.EmptyTypes)!;
        }

        /// <summary>
        /// Whether to string is overriden.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsToStringOverriden(this Type type)
        {
            return type.GetToStringMethod() != typeof(object).GetToStringMethod();
        }

        /// <summary>
        /// Tries to get static parse method with one string argument or dwo arguments (string and IFormatProvider). Returns null when no method was found.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="withFormatProvider"></param>
        /// <returns></returns>
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
    }
}