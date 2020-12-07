namespace Typin.Core.Internal.Exceptions
{
    using Typin.Exceptions;

    /// <summary>
    /// Internal exceptions. Provide more diagnostic information here.
    /// </summary>
    internal static class AttributesExceptions
    {
        public static TypinException DirectiveNameIsInvalid(string name)
        {
            string message = $@"
Directive name '[{name}]' is invalid.

Directives must have unique, non-empty names without whitespaces.
Names are not case-sensitive.";

            return new TypinException(message.Trim());
        }

        public static TypinException InvalidOptionName(string name)
        {
            string message = $@"Command option name '{name}' is invalid. 

Options must have a name starting from letter, while short names must be a letter.

Option names must be at least 2 characters long to avoid confusion with short names.
If you intended to set the short name instead, use the attribute overload that accepts a char.";

            return new TypinException(message.Trim());
        }

        public static TypinException InvalidOptionShortName(char shortName)
        {
            string message = $"Command option short name '{shortName}' is invalid. Options must have a name starting from letter, while short names must be a letter.";

            return new TypinException(message);
        }
    }
}