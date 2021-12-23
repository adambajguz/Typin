namespace Typin.Internal.Exceptions
{
    using System;
    using Typin.Exceptions;
    using Typin.Models.Converters;

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

            return new TemporaryException(message.Trim());
        }

        public static TypinException DirectiveNameIsInvalid(string name)
        {
            string message = $@"
Directive name '[{name}]' is invalid.

Directives must have unique, non-empty names without whitespaces.
Names are case-sensitive.";

            return new TemporaryException(message.Trim());
        }

        public static TypinException InvalidConverterType(Type converterType)
        {
            string message = $"Command argument has an invalid converter '{converterType.FullName}'. It must implement '{typeof(IArgumentConverter).Name}'.";

            return new TemporaryException(message);
        }
    }
}