namespace Typin.Utilities.Internal.Extensions
{
    internal static class StringExtensions
    {
        public static string? NullIfEmpty(this string? str)
        {
            return string.IsNullOrWhiteSpace(str) ? null : str;
        }
    }
}
