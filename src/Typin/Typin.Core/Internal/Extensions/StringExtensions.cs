namespace Typin.Internal.Extensions
{
    using System;
    using System.Collections.Generic;

    internal static class StringExtensions
    {
        public static string? NullIfEmpty(this string str)
        {
            return string.IsNullOrWhiteSpace(str) ? null : str;
        }

        public static string Quote(this string str)
        {
            return string.Concat("\"", str, "\"");
        }

        public static string JoinToString<T>(this IEnumerable<T> source, char separator)
        {
#if NETSTANDARD2_0
            return string.Join(separator.ToString(), source);
#else
            return string.Join(separator, source);
#endif
        }

        public static string JoinToString<T>(this IEnumerable<T> source, string separator)
        {
            return string.Join(separator, source);
        }

        public static string ToFormattableString(this object obj,
                                                 IFormatProvider? formatProvider = null,
                                                 string? format = null)
        {
            return obj is IFormattable formattable ? formattable.ToString(format, formatProvider) : obj.ToString() ?? string.Empty;
        }
    }
}