namespace Typin.Internal.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;

    internal static class StringExtensions
    {
        public static string Quote(this string str)
        {
            return string.Concat("\"", str, "\"");
        }

        public static string JoinToString<T>(this IEnumerable<T> source, string separator)
        {
            return string.Join(separator, source);
        }

        public static StringBuilder AppendIfNotEmpty(this StringBuilder builder, char value)
        {
            return builder.Length > 0 ? builder.Append(value) : builder;
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

        private static readonly Regex _splitRegex = new Regex(string.Format("{0}(?=(?:[^{1}]*{1}[^{1}]*{1})*(?![^{1}]*{1}))", Regex.Escape(" "), Regex.Escape("\"")),
                                                              RegexOptions.Compiled | RegexOptions.Multiline);

        //https://stackoverflow.com/questions/298830/split-string-containing-command-line-parameters-into-string-in-c-sharp
        //https://gist.github.com/Mikescher/a1450d13980f4363b47cdab5430b411a
        public static string[] SplitBySpacesWithEscape(this string expression)
        {
            var re = @"\G(""((""""|[^""])+)""|(\S+)) *";
            var ms = Regex.Matches(expression, re);
            var list = ms.Cast<Match>()
                         .Select(m => Regex.Replace(
                             m.Groups[2].Success
                                 ? m.Groups[2].Value
                                 : m.Groups[4].Value, @"""""", @"""")).ToArray();

            return list;
        }
    }
}