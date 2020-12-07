namespace Typin.Utilities
{
    using System;
    using System.Text;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Text utilities.
    /// </summary>
    public static class TextUtils
    {
        private static readonly Regex _newLinesRegex = new Regex(@"\r\n?|\n", RegexOptions.Compiled | RegexOptions.CultureInvariant);

        /// <summary>
        /// Replaces new line characters to match 'Environment.NewLine'.
        /// </summary>
        public static string AdjustNewLines(string text)
        {
            return _newLinesRegex.Replace(text, Environment.NewLine);
        }

        /// <summary>
        /// Converts tabs to spaces.
        /// </summary>
        public static string ConvertTabsToSpaces(string text, int width = 2)
        {
            return text.Replace("\t", new string(' ', width));
        }

        /// <summary>
        /// Convertes escaped char sequence to char.
        /// The following escape sequences are supported: '\0', '\a', '\b', '\f', '\n', '\r', '\t', '\v', and '\\')
        /// </summary>
        public static char UnescapeChar(this string? str)
        {
            if (string.IsNullOrWhiteSpace(str))
                return '\0';

            if (str.Length == 1)
                return str[0];

            if (str.Length == 2 && str[0] == '\\')
            {
                return (str[1]) switch
                {
                    '0' => '\0', // Null
                    'a' => '\a', // Alert
                    'b' => '\b', // Backspace
                    'f' => '\f', // Form feed
                    'n' => '\n', // Line feed
                    'r' => '\r', // Carriage return
                    't' => '\t', // Tab
                    'v' => '\v', // Vertical tab
                    '\\' => '\\', // Don't escape

                    _ => throw new FormatException($"Cannot parse '{str}' to char. Unrecognized escape sequences can be converted. " +
                                                   $"Only the following escape sequences are supported: '\\0', '\\a', '\\b', '\\f', '\\n', '\\r', '\\t', '\\v', and '\\\\')"),
                };
            }

            throw new FormatException($"Cannot parse '{str}' to char. Only one letter string or escape sequences can be converted. " +
                                      $"Only the following escape sequences are supported: ('\\0', '\\a', '\\b', '\\f', '\\n', '\\r', '\\t', '\\v', and '\\\\')");
        }
    }
}