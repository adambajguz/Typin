namespace Typin.Internal.Exceptions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Typin.Exceptions;
    using Typin.Input;
    using Typin.Internal.Extensions;
    using Typin.Schemas;

    /// <summary>
    /// End-user-facing exceptions. Avoid internal details and fix recommendations here
    /// </summary>
    internal static class EndUserTypinExceptions
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
Command '{command.Type.FullName}' can be executed only in interactive mode, but this application is using {nameof(CliApplication)}.

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
Command '{command.Type.FullName}' can be executed only in interactive mode, but this application is not running in this mode.

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