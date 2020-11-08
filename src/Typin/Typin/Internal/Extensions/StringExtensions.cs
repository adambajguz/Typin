namespace Typin.Internal.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    internal static class StringExtensions
    {
        private static readonly Regex _toLowerCaseRegex = new Regex("[A-Z]", RegexOptions.Compiled | RegexOptions.CultureInvariant);
        private static readonly Regex _toCamelCaseRegex = new Regex("_[a-z]", RegexOptions.Compiled | RegexOptions.CultureInvariant);

        public static string ToHyphenCase(this string s)
        {
            return _toLowerCaseRegex.Replace(s, "-$0")
                                    .TrimStart('-')
                                    .Replace('_', '-')
                                    .ToLower();
        }

        public static string ToSnakeCase(this string s)
        {
            return _toLowerCaseRegex.Replace(s, "_$0")
                                    .ToLower();
        }

        public static string ToCamelCase(this string s1)
        {
            return _toCamelCaseRegex.Replace(s1, delegate (Match m)
            {
                return m.ToString().TrimStart('_').ToUpper();
            });
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
            return obj is IFormattable formattable ? formattable.ToString(format, formatProvider) : obj.ToString();
        }

        public static string PadBoth(this string source, int length, char paddingChar = ' ')
        {
            int spaces = length - source.Length;
            int padLeft = spaces / 2 + source.Length;

            return source.PadLeft(padLeft, paddingChar)
                         .PadRight(length, paddingChar);
        }
    }
}