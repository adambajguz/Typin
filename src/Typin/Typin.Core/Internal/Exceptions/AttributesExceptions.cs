namespace Typin.Internal.Exceptions
{
    using System;
    using Typin.Binding;
    using Typin.Exceptions;

    /// <summary>
    /// Internal exceptions. Provide more diagnostic information here.
    /// </summary>
    internal static class AttributesExceptions
    {
        public static TypinException InvalidModeType(Type type)
        {
            string message = $@"
CLI mode '{type.FullName}' is not a valid CLI mode type.

In order to be a valid CLI mode type, it must:
- Not be an abstract class
- Implement {typeof(ICliMode).FullName}.";

            return new TypinException(message.Trim());
        }

        public static TypinException DirectiveNameIsInvalid(string name)
        {
            string message = $@"
Directive name '[{name}]' is invalid.

Directives must have unique, non-empty names without whitespaces.
Names are case-sensitive.";

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
        
        public static TypinException InvalidConverterType(Type converterType)
        {
            string message = $"Command argument has an invalid converter '{converterType.FullName}'. It must implement '{typeof(IBindingConverter).Name}'.";

            return new TypinException(message);
        }  
    }
}