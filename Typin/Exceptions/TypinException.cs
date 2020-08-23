namespace Typin.Exceptions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Typin.Attributes;
    using Typin.Input;
    using Typin.Internal.Extensions;
    using Typin.Schemas;

    /// <summary>
    /// Domain exception thrown within Typin.
    /// </summary>
    public partial class TypinException : Exception
    {
        private readonly bool _isMessageSet;

        /// <summary>
        /// Whether to show the help text after handling this exception.
        /// </summary>
        public bool ShowHelp { get; set; } = true;

        /// <summary>
        /// Initializes an instance of <see cref="TypinException"/> with a specified error message.
        /// </summary>
        public TypinException(string? message)
            : base(message)
        {
            _isMessageSet = !string.IsNullOrWhiteSpace(message);
        }

        /// <summary>
        /// Initializes an instance of <see cref="TypinException"/> with a specified error
        /// message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        public TypinException(string? message, Exception? innerException)
            : base(message, innerException!)
        {
            _isMessageSet = !string.IsNullOrWhiteSpace(message);
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return _isMessageSet ? Message : base.ToString();
        }
    }

    // Internal exceptions
    // Provide more diagnostic information here
    public partial class TypinException
    {
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

    // End-user-facing exceptions
    // Avoid internal details and fix recommendations here
    public partial class TypinException
    {
        internal static TypinException CannotConvertMultipleValuesToNonScalar(
            CommandParameterSchema parameter,
            IReadOnlyList<string> values)
        {
            var message = $@"
Parameter {parameter.GetUserFacingDisplayString()} expects a single value, but provided with multiple:
{values.Select(v => v.Quote()).JoinToString(" ")}";

            return new TypinException(message.Trim());
        }

        internal static TypinException CannotConvertMultipleValuesToNonScalar(
            CommandOptionSchema option,
            IReadOnlyList<string> values)
        {
            var message = $@"
Option {option.GetUserFacingDisplayString()} expects a single value, but provided with multiple:
{values.Select(v => v.Quote()).JoinToString(" ")}";

            return new TypinException(message.Trim());
        }

        internal static TypinException CannotConvertMultipleValuesToNonScalar(
            ArgumentSchema argument,
            IReadOnlyList<string> values)
        {
            return argument switch
            {
                CommandParameterSchema parameter => CannotConvertMultipleValuesToNonScalar(parameter, values),
                CommandOptionSchema option => CannotConvertMultipleValuesToNonScalar(option, values),
                _ => throw new ArgumentOutOfRangeException(nameof(argument))
            };
        }

        internal static TypinException CannotConvertToType(
            CommandParameterSchema parameter,
            string? value,
            Type type,
            Exception? innerException = null)
        {
            var message = $@"
Can't convert value ""{value ?? "<null>"}"" to type '{type.Name}' for parameter {parameter.GetUserFacingDisplayString()}.
{innerException?.Message ?? "This type is not supported."}";

            return new TypinException(message.Trim(), innerException);
        }

        internal static TypinException CannotConvertToType(
            CommandOptionSchema option,
            string? value,
            Type type,
            Exception? innerException = null)
        {
            var message = $@"
Can't convert value ""{value ?? "<null>"}"" to type '{type.Name}' for option {option.GetUserFacingDisplayString()}.
{innerException?.Message ?? "This type is not supported."}";

            return new TypinException(message.Trim(), innerException);
        }

        internal static TypinException CannotConvertToType(
            ArgumentSchema argument,
            string? value,
            Type type,
            Exception? innerException = null)
        {
            return argument switch
            {
                CommandParameterSchema parameter => CannotConvertToType(parameter, value, type, innerException),
                CommandOptionSchema option => CannotConvertToType(option, value, type, innerException),
                _ => throw new ArgumentOutOfRangeException(nameof(argument))
            };
        }

        internal static TypinException CannotConvertNonScalar(
            CommandParameterSchema parameter,
            IReadOnlyList<string> values,
            Type type)
        {
            var message = $@"
Can't convert provided values to type '{type.Name}' for parameter {parameter.GetUserFacingDisplayString()}:
{values.Select(v => v.Quote()).JoinToString(" ")}

Target type is not assignable from array and doesn't have a public constructor that takes an array.";

            return new TypinException(message.Trim());
        }

        internal static TypinException CannotConvertNonScalar(
            CommandOptionSchema option,
            IReadOnlyList<string> values,
            Type type)
        {
            var message = $@"
Can't convert provided values to type '{type.Name}' for option {option.GetUserFacingDisplayString()}:
{values.Select(v => v.Quote()).JoinToString(" ")}

Target type is not assignable from array and doesn't have a public constructor that takes an array.";

            return new TypinException(message.Trim());
        }

        internal static TypinException CannotConvertNonScalar(
            ArgumentSchema argument,
            IReadOnlyList<string> values,
            Type type)
        {
            return argument switch
            {
                CommandParameterSchema parameter => CannotConvertNonScalar(parameter, values, type),
                CommandOptionSchema option => CannotConvertNonScalar(option, values, type),
                _ => throw new ArgumentOutOfRangeException(nameof(argument))
            };
        }

        internal static TypinException ParameterNotSet(CommandParameterSchema parameter)
        {
            var message = $@"
Missing value for parameter {parameter.GetUserFacingDisplayString()}.";

            return new TypinException(message.Trim());
        }

        internal static TypinException RequiredOptionsNotSet(IReadOnlyList<CommandOptionSchema> options)
        {
            var message = $@"
Missing values for one or more required options:
{options.Select(o => o.GetUserFacingDisplayString()).JoinToString(Environment.NewLine)}";

            return new TypinException(message.Trim());
        }

        internal static TypinException UnrecognizedParametersProvided(IReadOnlyList<CommandParameterInput> parameterInputs)
        {
            var message = $@"
Unrecognized parameters provided:
{parameterInputs.Select(p => p.Value).JoinToString(Environment.NewLine)}";

            return new TypinException(message.Trim());
        }

        internal static TypinException UnrecognizedOptionsProvided(IReadOnlyList<CommandOptionInput> optionInputs)
        {
            var message = $@"
Unrecognized options provided:
{optionInputs.Select(o => o.GetRawAlias()).JoinToString(Environment.NewLine)}";

            return new TypinException(message.Trim());
        }

        internal static TypinException InteractiveOnlyCommandButThisIsNormalApplication(CommandSchema command)
        {
            var message = $@"
Command '{command.Type.FullName}' can be executed only in interactive mode, but this is a application is using {nameof(CliApplication)}.

Please consider switching to interactive application or removing the command.";

            return new TypinException(message.Trim());
        }

        internal static TypinException UnknownDirectiveName(DirectiveInput directive)
        {
            var message = $@"
Unknown directive '{directive}'.";

            return new TypinException(message.Trim());
        }

        internal static TypinException InteractiveOnlyCommandButInteractiveModeNotStarted(CommandSchema command)
        {
            var message = $@"
Command '{command.Type.FullName}' can be executed only in interactive mode, but this is a application is not running in this mode.

You can start the interactive mode with [{BuiltInDirectives.Interactive}].";

            return new TypinException(message.Trim());
        }

        internal static TypinException InteractiveModeNotSupported()
        {
            var message = $@"
This application does not support interactive mode.";

            return new TypinException(message.Trim());
        }

        internal static TypinException InteractiveModeDirectiveNotAvailable(string directiveName)
        {
            var message = $@"
Directive '[{directiveName}]' is for interactive mode only. Thus, cannot be used in normal mode.

You can start the interactive mode with [{BuiltInDirectives.Interactive}].";

            return new TypinException(message.Trim());
        }
    }
}