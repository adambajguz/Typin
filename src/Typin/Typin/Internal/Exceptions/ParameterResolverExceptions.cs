namespace Typin.Internal.Exceptions
{
    using System;
    using System.Collections.Generic;
    using Typin.Exceptions;
    using Typin.Internal.Extensions;
    using Typin.Schemas;

    /// <summary>
    /// Internal exceptions. Provide more diagnostic information here.
    /// </summary>
    internal static class ParameterResolverExceptions
    {
        public static TypinException ParametersWithSameOrder(CommandSchema command, int order, IReadOnlyList<CommandParameterSchema> invalidParameters)
        {
            string message = $@"
Command '{command.Type.FullName}' is invalid because it contains {invalidParameters.Count} parameters with the same order ({order}):
{invalidParameters.JoinToString(Environment.NewLine)}

Parameters must have unique order.";

            return new TypinException(message.Trim());
        }

        public static TypinException ParametersWithSameName(CommandSchema command, string name, IReadOnlyList<CommandParameterSchema> invalidParameters)
        {
            string message = $@"
Command '{command.Type.FullName}' is invalid because it contains {invalidParameters.Count} parameters with the same name ('{name}'):
{invalidParameters.JoinToString(Environment.NewLine)}

Parameters must have unique names to avoid potential confusion in the help text (names are case-insensitive).";

            return new TypinException(message.Trim());
        }

        public static TypinException TooManyNonScalarParameters(CommandSchema command, IReadOnlyList<CommandParameterSchema> invalidParameters)
        {
            string message = $@"
Command '{command.Type.FullName}' is invalid because it contains {invalidParameters.Count} non-scalar parameters:
{invalidParameters.JoinToString(Environment.NewLine)}

Non-scalar parameter is such that is bound from more than one value (e.g. array).
Only one parameter in a command may be non-scalar and it must be the last one in order.

If it's not feasible to fit into these constraints, consider using options instead as they don't have these limitations.";

            return new TypinException(message.Trim());
        }

        public static TypinException NonLastNonScalarParameter(CommandSchema command, CommandParameterSchema invalidParameter)
        {
            string message = $@"
Command '{command.Type.FullName}' is invalid because it contains a non-scalar parameter which is not the last in order:
{invalidParameter}

Non-scalar parameter is such that is bound from more than one value (e.g. array).
Only one parameter in a command may be non-scalar and it must be the last one in order.

If it's not feasible to fit into these constraints, consider using options instead as they don't have these limitations.";

            return new TypinException(message.Trim());
        }
    }
}