namespace Typin.Internal.Exceptions
{
    using System;
    using System.Collections.Generic;
    using Typin.Attributes;
    using Typin.AutoCompletion;
    using Typin.Exceptions;
    using Typin.Internal.Extensions;
    using Typin.Schemas;

    /// <summary>
    /// Internal exceptions. Provide more diagnostic information here.
    /// </summary>
    internal static class ResolversExceptions
    {
        internal static TypinException DuplicatedShortcut(ShortcutDefinition definition)
        {
            var message = $@"
Shortcut '{definition.Modifiers}+{definition.Key}' is already used by Typin.";

            return new TypinException(message.Trim());
        }

        internal static TypinException InvalidCommandType(Type type)
        {
            var message = $@"
Command '{type.FullName}' is not a valid command type.

In order to be a valid command type, it must:
- Not be an abstract class
- Implement {typeof(ICommand).FullName}
- Be annotated with {typeof(CommandAttribute).FullName}

If you're experiencing problems, please refer to the readme for a quickstart example.";

            return new TypinException(message.Trim());
        }

        internal static TypinException InvalidDirectiveType(Type type)
        {
            var message = $@"
Directive '{type.FullName}' is not a valid directive type.

In order to be a valid command type, it must:
- Not be an abstract class
- Implement {typeof(IDirective).FullName}
- Be annotated with {typeof(DirectiveAttribute).FullName}

If you're experiencing problems, please refer to the readme for a quickstart example.";

            return new TypinException(message.Trim());
        }

        internal static TypinException NoCommandsDefined()
        {
            var message = $@"
There are no commands configured in the application.

To fix this, ensure that at least one command is added through one of the methods on {nameof(CliApplicationBuilder)}.
If you're experiencing problems, please refer to the readme for a quickstart example.";

            return new TypinException(message.Trim());
        }

        internal static TypinException TooManyDefaultCommands()
        {
            var message = $@"
Application configuration is invalid because there are too many default commands.

There can only be one default command (i.e. command with no name) in an application.
Other commands must have unique non-empty names that identify them.";

            return new TypinException(message.Trim());
        }

        internal static TypinException CommandsWithSameName(
            string name,
            IReadOnlyList<CommandSchema> invalidCommands)
        {
            var message = $@"
Application configuration is invalid because there are {invalidCommands.Count} commands with the same name ('{name}'):
{invalidCommands.JoinToString(Environment.NewLine)}

Commands must have unique names.
Names are not case-sensitive.";

            return new TypinException(message.Trim());
        }

        internal static TypinException DirectiveWithSameName(
            string name,
            IReadOnlyList<DirectiveSchema> invalidDirectives)
        {
            var message = $@"
Application configuration is invalid because there are {invalidDirectives.Count} directives with the same name ('[{name}]'):
{invalidDirectives.JoinToString(Environment.NewLine)}

Directives must have unique names.
Names are not case-sensitive.";

            return new TypinException(message.Trim());
        }

        internal static TypinException DirectiveNameIsInvalid(
            string name, Type type)
        {
            var message = $@"
Directive '{type.FullName}' is invalid because its name is empty or whitespace ('[{name}]').

Directives must have unique, non-empty, and non-whitespace names.
Names are not case-sensitive.";

            return new TypinException(message.Trim());
        }

        internal static TypinException ParametersWithSameOrder(
            CommandSchema command,
            int order,
            IReadOnlyList<CommandParameterSchema> invalidParameters)
        {
            var message = $@"
Command '{command.Type.FullName}' is invalid because it contains {invalidParameters.Count} parameters with the same order ({order}):
{invalidParameters.JoinToString(Environment.NewLine)}

Parameters must have unique order.";

            return new TypinException(message.Trim());
        }

        internal static TypinException ParametersWithSameName(
            CommandSchema command,
            string name,
            IReadOnlyList<CommandParameterSchema> invalidParameters)
        {
            var message = $@"
Command '{command.Type.FullName}' is invalid because it contains {invalidParameters.Count} parameters with the same name ('{name}'):
{invalidParameters.JoinToString(Environment.NewLine)}

Parameters must have unique names to avoid potential confusion in the help text.
Names are not case-sensitive.";

            return new TypinException(message.Trim());
        }

        internal static TypinException TooManyNonScalarParameters(
            CommandSchema command,
            IReadOnlyList<CommandParameterSchema> invalidParameters)
        {
            var message = $@"
Command '{command.Type.FullName}' is invalid because it contains {invalidParameters.Count} non-scalar parameters:
{invalidParameters.JoinToString(Environment.NewLine)}

Non-scalar parameter is such that is bound from more than one value (e.g. array).
Only one parameter in a command may be non-scalar and it must be the last one in order.

If it's not feasible to fit into these constraints, consider using options instead as they don't have these limitations.";

            return new TypinException(message.Trim());
        }

        internal static TypinException NonLastNonScalarParameter(
            CommandSchema command,
            CommandParameterSchema invalidParameter)
        {
            var message = $@"
Command '{command.Type.FullName}' is invalid because it contains a non-scalar parameter which is not the last in order:
{invalidParameter}

Non-scalar parameter is such that is bound from more than one value (e.g. array).
Only one parameter in a command may be non-scalar and it must be the last one in order.

If it's not feasible to fit into these constraints, consider using options instead as they don't have these limitations.";

            return new TypinException(message.Trim());
        }

        internal static TypinException OptionsWithDigitStartingName(
            CommandSchema command,
            IReadOnlyList<CommandOptionSchema> invalidOptions)
        {
            var message = $@"
Command '{command.Type.FullName}' is invalid because it contains one or more options with a name starting from digit or short names with a digit:
{invalidOptions.JoinToString(Environment.NewLine)}

Options must have a name starting from char other than digit, while short name must not be a digit.";

            return new TypinException(message.Trim());
        }

        internal static TypinException OptionsWithNoName(
            CommandSchema command,
            IReadOnlyList<CommandOptionSchema> invalidOptions)
        {
            var message = $@"
Command '{command.Type.FullName}' is invalid because it contains one or more options without a name:
{invalidOptions.JoinToString(Environment.NewLine)}

Options must have either a name or a short name or both.";

            return new TypinException(message.Trim());
        }

        internal static TypinException OptionsWithInvalidLengthName(
            CommandSchema command,
            IReadOnlyList<CommandOptionSchema> invalidOptions)
        {
            var message = $@"
Command '{command.Type.FullName}' is invalid because it contains one or more options whose names are too short:
{invalidOptions.JoinToString(Environment.NewLine)}

Option names must be at least 2 characters long to avoid confusion with short names.
If you intended to set the short name instead, use the attribute overload that accepts a char.";

            return new TypinException(message.Trim());
        }

        internal static TypinException OptionsWithSameName(
            CommandSchema command,
            string name,
            IReadOnlyList<CommandOptionSchema> invalidOptions)
        {
            var message = $@"
Command '{command.Type.FullName}' is invalid because it contains {invalidOptions.Count} options with the same name ('{name}'):
{invalidOptions.JoinToString(Environment.NewLine)}

Options must have unique names.
Names are not case-sensitive.";

            return new TypinException(message.Trim());
        }

        internal static TypinException OptionsWithSameShortName(
            CommandSchema command,
            char shortName,
            IReadOnlyList<CommandOptionSchema> invalidOptions)
        {
            var message = $@"
Command '{command.Type.FullName}' is invalid because it contains {invalidOptions.Count} options with the same short name ('{shortName}'):
{invalidOptions.JoinToString(Environment.NewLine)}

Options must have unique short names.
Short names are case-sensitive (i.e. 'a' and 'A' are different short names).";

            return new TypinException(message.Trim());
        }

        internal static TypinException OptionsWithSameEnvironmentVariableName(
            CommandSchema command,
            string environmentVariableName,
            IReadOnlyList<CommandOptionSchema> invalidOptions)
        {
            var message = $@"
Command '{command.Type.FullName}' is invalid because it contains {invalidOptions.Count} options with the same fallback environment variable name ('{environmentVariableName}'):
{invalidOptions.JoinToString(Environment.NewLine)}

Options cannot share the same environment variable as a fallback.
Environment variable names are not case-sensitive.";

            return new TypinException(message.Trim());
        }
    }
}