namespace Typin.Internal.Extensions
{
    using System.Collections.Generic;
    using System.Text;

    internal static class StringExtensions
    {
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

        public static StringBuilder AppendIfNotEmpty(this StringBuilder builder, char value)
        {
            return builder.Length > 0 ? builder.Append(value) : builder;
        }
    }
}