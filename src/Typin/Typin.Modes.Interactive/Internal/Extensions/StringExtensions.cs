namespace Typin.Modes.Interactive.Internal.Extensions
{
    using System.Collections.Generic;

    internal static class StringExtensions
    {
        public static string JoinToString<T>(this IEnumerable<T> source, char separator)
        {
            return string.Join(separator, source);
        }
    }
}