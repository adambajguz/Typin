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
    /// Argument binding end-user-facing exceptions. Avoid internal details and fix recommendations here
    /// </summary>
    internal static class ArgumentBindingExceptions
    {
        public static TypinException UnknownDirectiveName(DirectiveInput directive)
        {
            string message = $@"Unknown directive '{directive}'.";

            return new TypinException(message);
        }

        public static TypinException CannotConvertMultipleValuesToNonScalar(CommandParameterSchema parameter, IReadOnlyCollection<string> values)
        {
            string message = $@"
Parameter {parameter} expects a single value, but provided with multiple:
{values.Select(v => v.Quote()).JoinToString(' ')}";

            return new TypinException(message.Trim());
        }

        public static TypinException CannotConvertMultipleValuesToNonScalar(CommandOptionSchema option, IReadOnlyCollection<string> values)
        {
            string message = $@"
Option {option} expects a single value, but provided with multiple:
{values.Select(v => v.Quote()).JoinToString(' ')}";

            return new TypinException(message.Trim());
        }

        public static TypinException CannotConvertMultipleValuesToNonScalar(ArgumentSchema argument, IReadOnlyCollection<string> values)
        {
            return argument switch
            {
                CommandParameterSchema parameter => CannotConvertMultipleValuesToNonScalar(parameter, values),
                CommandOptionSchema option => CannotConvertMultipleValuesToNonScalar(option, values),
                _ => throw new ArgumentOutOfRangeException(nameof(argument))
            };
        }

        public static TypinException CannotConvertToType(CommandParameterSchema parameter,
                                                         string? value,
                                                         Type type,
                                                         Exception? innerException = null)
        {
            string message = $@"
Can't convert value ""{value ?? "<null>"}"" to type '{type.Name}' for parameter {parameter}.
{innerException?.Message ?? "This type is not supported."}";

            return new TypinException(message.Trim(), innerException);
        }

        public static TypinException CannotConvertToType(CommandOptionSchema option,
                                                         string? value,
                                                         Type type,
                                                         Exception? innerException = null)
        {
            string message = $@"
Can't convert value ""{value ?? "<null>"}"" to type '{type.Name}' for option {option}.
{innerException?.Message ?? "This type is not supported."}";

            return new TypinException(message.Trim(), innerException);
        }

        public static TypinException CannotConvertToType(ArgumentSchema argument,
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

        public static TypinException CannotConvertNonScalar(CommandParameterSchema parameter,
                                                            IReadOnlyCollection<string> values,
                                                            Type type)
        {
            string message = $@"
Can't convert provided values to type '{type.Name}' for parameter {parameter}:
{values.Select(v => v.Quote()).JoinToString(' ')}

Target type is not assignable from array and doesn't have a public constructor that takes an array.";

            return new TypinException(message.Trim());
        }

        public static TypinException CannotConvertNonScalar(CommandOptionSchema option,
                                                            IReadOnlyCollection<string> values,
                                                            Type type)
        {
            string message = $@"
Can't convert provided values to type '{type.Name}' for option {option}:
{values.Select(v => v.Quote()).JoinToString(' ')}

Target type is not assignable from array and doesn't have a public constructor that takes an array.";

            return new TypinException(message.Trim());
        }

        public static TypinException CannotConvertNonScalar(ArgumentSchema argument,
                                                            IReadOnlyCollection<string> values,
                                                            Type type)
        {
            return argument switch
            {
                CommandParameterSchema parameter => CannotConvertNonScalar(parameter, values, type),
                CommandOptionSchema option => CannotConvertNonScalar(option, values, type),
                _ => throw new ArgumentOutOfRangeException(nameof(argument))
            };
        }

        public static TypinException ParameterNotSet(IEnumerable<CommandParameterSchema> parameter)
        {
            string message = $@"Missing value for parameter: {parameter.Select(x => x.ToString()).JoinToString(", ")}.";

            return new TypinException(message);
        }

        public static TypinException RequiredOptionsNotSet(IEnumerable<CommandOptionSchema> options)
        {
            string message = $@"Missing values for one or more required options: {options.Select(o => o).JoinToString(", ")}";

            return new TypinException(message);
        }

        public static TypinException UnrecognizedParametersProvided(IEnumerable<CommandParameterInput> parameterInputs)
        {
            string message = $@"Unrecognized parameters provided: {parameterInputs.Select(p => p.Value).JoinToString(", ")}";

            return new TypinException(message);
        }

        public static TypinException UnrecognizedOptionsProvided(IEnumerable<CommandOptionInput> optionInputs)
        {
            string message = $@"Unrecognized options provided: {optionInputs.Select(o => o.GetRawAlias()).JoinToString(", ")}";

            return new TypinException(message);
        }
    }
}