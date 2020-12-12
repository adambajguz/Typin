namespace Typin.Extensions
{
    using System.Text.RegularExpressions;

    /// <summary>
    /// String extensions.
    /// </summary>
    public static class StringExtensions
    {
        private static readonly Regex _toLowerCaseRegex = new Regex("[A-Z]", RegexOptions.Compiled | RegexOptions.CultureInvariant);
        private static readonly Regex _toCamelCaseRegex = new Regex("_[a-z]", RegexOptions.Compiled | RegexOptions.CultureInvariant);

        /// <summary>
        /// Converts string to Kebab case.
        /// </summary>
        public static string ToKebabCase(this string s)
        {
            return _toLowerCaseRegex.Replace(s, "-$0")
                                    .TrimStart('-')
                                    .Replace('_', '-')
                                    .ToLower();
        }

        /// <summary>
        /// Converts string to Snake case.
        /// </summary>
        public static string ToSnakeCase(this string s)
        {
            return _toLowerCaseRegex.Replace(s, "_$0")
                                    .ToLower();
        }

        /// <summary>
        /// Converts string to Camel case.
        /// </summary>
        public static string ToCamelCase(this string s1)
        {
            return _toCamelCaseRegex.Replace(s1, delegate (Match m)
            {
                return m.ToString().TrimStart('_').ToUpper();
            });
        }

        /// <summary>
        /// Returns a new string that right-aligns the characters in this instance by padding them on the left and right with a specified Unicode character, for a specified total length.
        /// </summary>
        public static string PadBoth(this string source, int totalWidth, char paddingChar = ' ')
        {
            int spaces = totalWidth - source.Length;
            int padLeft = spaces / 2 + source.Length;

            return source.PadLeft(padLeft, paddingChar)
                         .PadRight(totalWidth, paddingChar);
        }
    }
}