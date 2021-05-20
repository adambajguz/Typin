namespace Typin.Utilities
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Text utilities.
    /// </summary>
    public static class TextUtils
    {
        private static readonly Regex _newLinesRegex = new(@"\r\n?|\n", RegexOptions.Compiled | RegexOptions.CultureInvariant);
        private static readonly Regex _toKebabCaseRegex = new(@"[A-Z]{2,}(?=[A-Z][a-z]+[0-9]*|\b)|[A-Z]?[a-z]+[0-9]*|[A-Z]|[0-9]+", RegexOptions.Compiled | RegexOptions.CultureInvariant);

        /// <summary>
        /// Converts string to Kebab case.
        /// </summary>
        public static string ToKebabCase(string text)
        {
            return string.Join("-", _toKebabCaseRegex.Matches(text)).ToLower();
        }

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
        /// Converts escaped char sequence to char.
        /// The following escape sequences are supported: '\0', '\a', '\b', '\f', '\n', '\r', '\t', '\v', '\\', and Unicode escape sequence e.g. \u006A)
        /// </summary>
        [SuppressMessage("Style", "IDE0057:Use range operator")]
        public static char UnescapeChar(string? text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return '\0';
            }

            if (text.Length == 1)
            {
                return text[0];
            }

            if (text[0] == '\\')
            {
                if (text.Length == 2)
                {
                    return (text[1]) switch
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

                        _ => throw new FormatException($"Cannot parse '{text}' to char. Unrecognized escape sequences can be converted. " +
                                                       $"Only the following escape sequences are supported: '\\0', '\\a', '\\b', '\\f', '\\n', '\\r', '\\t', '\\v', '\\\\', and Unicode escape e.g. \u006A)"),
                    };
                }
                else if (text.Length == 6 && char.ToLowerInvariant(text[1]) == 'u')
                {
                    if (int.TryParse(text.Substring(2), NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture, out int result))
                    {
                        return (char)result;
                    }
                }
            }

            throw new FormatException($"Cannot parse '{text}' to char. Only one letter string or escape sequences can be converted. " +
                                      $"Only the following escape sequences are supported: ('\\0', '\\a', '\\b', '\\f', '\\n', '\\r', '\\t', '\\v', '\\\\', and Unicode escape e.g. \u006A)");
        }
    }
}