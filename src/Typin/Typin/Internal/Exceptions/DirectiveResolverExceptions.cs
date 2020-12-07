namespace Typin.Internal.Exceptions
{
    using System;
    using System.Collections.Generic;
    using Typin.Attributes;
    using Typin.Exceptions;
    using Typin.Internal.Extensions;
    using Typin.Schemas;

    /// <summary>
    /// Internal exceptions. Provide more diagnostic information here.
    /// </summary>
    internal static class DirectiveResolverExceptions
    {
        public static TypinException InvalidSupportedModesInDirective(Type type)
        {
            string message = $@"
Directive '{type.FullName}' contains an invalid mode in SupportedModes parameter.
Either the type does not implement {nameof(ICliMode)} or CLI mode was not registered.

If you're experiencing problems, please refer to the readme for a quickstart example.";

            return new TypinException(message.Trim());
        }

        public static TypinException InvalidDirectiveType(Type type)
        {
            string message = $@"
Directive '{type.FullName}' is not a valid directive type.

In order to be a valid directive type, it must:
- Not be an abstract class
- Implement {typeof(IDirective).FullName}
- Be annotated with {typeof(DirectiveAttribute).FullName}

If you're experiencing problems, please refer to the readme for a quickstart example.";

            return new TypinException(message.Trim());
        }

        public static TypinException DirectiveWithSameName(string name, IReadOnlyList<DirectiveSchema> invalidDirectives)
        {
            string message = $@"
Application configuration is invalid because there are {invalidDirectives.Count} directives with the same name ('[{name}]'):
{invalidDirectives.JoinToString(Environment.NewLine)}

Directives must have unique names.
Names are not case-sensitive.";

            return new TypinException(message.Trim());
        }
    }
}