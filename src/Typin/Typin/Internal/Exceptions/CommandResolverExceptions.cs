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
    internal static class CommandResolverExceptions
    {
        public static TypinException InvalidSupportedModesInCommand(Type type, CommandAttribute attribute)
        {
            string message = $@"
Command '{type.FullName}' contains invalid supported mode(s) ({attribute.SupportedModes}).
Either the type does not implement {nameof(ICliMode)} or CLI mode was not registered.";

            return new TypinException(message.Trim());
        }

        public static TypinException InvalidExcludedModesInCommand(Type type, CommandAttribute attribute)
        {
            string message = $@"
Command '{type.FullName}' contains invalid excluded mode(s) ({attribute.SupportedModes}).
Either the type does not implement {nameof(ICliMode)} or CLI mode was not registered.";

            return new TypinException(message.Trim());
        }

        public static TypinException InvalidCommandType(Type type)
        {
            string message = $@"
Command '{type.FullName}' is not a valid command type.

In order to be a valid command type, it must:
- Not be an abstract class
- Implement {typeof(ICommand).FullName}
- Be annotated with {typeof(CommandAttribute).FullName}.";

            return new TypinException(message.Trim());
        }

        public static TypinException NoCommandsDefined()
        {
            string message = $@"
There are no commands configured in the application.

To fix this, ensure that at least one command is added through one of the methods on {nameof(CliApplicationBuilder)}.";

            return new TypinException(message.Trim());
        }

        public static TypinException TooManyDefaultCommands()
        {
            string message = $@"
Application configuration is invalid because there are too many default commands.

There can only be one default command (i.e. command with no name) in an application.
Other commands must have unique non-empty names that identify them.";

            return new TypinException(message.Trim());
        }

        public static TypinException CommandsWithSameName(string name, IReadOnlyList<CommandSchema> invalidCommands)
        {
            string message = $@"
Application configuration is invalid because there are {invalidCommands.Count} commands with the same name ('{name}'):
{invalidCommands.JoinToString(Environment.NewLine)}

Commands must have unique names (names are case-insensitive).";

            return new TypinException(message.Trim());
        }
    }
}