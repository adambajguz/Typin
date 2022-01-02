namespace Typin.Utilities.Extensions
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// <see cref="string"/> extensions.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Returns a new string that right-aligns the characters in this instance by
        /// padding them on the left and right with a specified Unicode character, for a specified total length.
        /// </summary>
        public static string PadBoth(this string source, int totalWidth, char paddingChar = ' ')
        {
            int spaces = totalWidth - source.Length;
            int padLeft = spaces / 2 + source.Length;

            return source.PadLeft(padLeft, paddingChar)
                         .PadRight(totalWidth, paddingChar);
        }

        /// <summary>
        /// Quotes a <paramref name="str"/>.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Quote(this string str)
        {
            return string.Concat("\"", str, "\"");
        }

        /// <summary>
        /// Joins <paramref name="source"/> using a <paramref name="separator"/> as a separator.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static string JoinToString<T>(this IEnumerable<T> source, char separator)
        {
            return string.Join(separator, source);
        }

        /// <summary>
        /// Joins <paramref name="source"/> using a <paramref name="separator"/> as a separator.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static string JoinToString<T>(this IEnumerable<T> source, string separator)
        {
            return string.Join(separator, source);
        }

        /// <summary>
        /// Invokes <see cref="IFormattable.ToString(string?, IFormatProvider?)"/> or
        /// <see cref="object.ToString()"/> when <paramref name="obj"/> does not implement <see cref="IFormattable"/>.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="formatProvider"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static string ToFormattableString(this object obj,
                                                 IFormatProvider? formatProvider = null,
                                                 string? format = null)
        {
            return obj is IFormattable formattable ?
                formattable.ToString(format, formatProvider) :
                obj.ToString() ?? string.Empty;
        }
    }
}