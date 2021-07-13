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
    internal static class OptionResolverExceptions
    {
        public static TypinException OptionsWithSameName(BaseCommandSchema command, string name, IReadOnlyList<OptionSchema> invalidOptions)
        {
            string message = $@"
Command '{command.Type.FullName}' is invalid because it contains {invalidOptions.Count} options with the same name ('{name}'):
{invalidOptions.JoinToString(Environment.NewLine)}

Options must have unique names (names are case-sensitive).";

            return new TypinException(message.Trim());
        }

        public static TypinException OptionsWithSameShortName(BaseCommandSchema command, char shortName, IReadOnlyList<OptionSchema> invalidOptions)
        {
            string message = $@"
Command '{command.Type.FullName}' is invalid because it contains {invalidOptions.Count} options with the same short name ('{shortName}'):
{invalidOptions.JoinToString(Environment.NewLine)}

Options must have unique short names. Short names are case-sensitive (i.e. 'a' and 'A' are different short names).";

            return new TypinException(message.Trim());
        }

        public static TypinException OptionsWithSameEnvironmentVariableName(BaseCommandSchema command, string environmentVariableName, IReadOnlyList<OptionSchema> invalidOptions)
        {
            string message = $@"
Command '{command.Type.FullName}' is invalid because it contains {invalidOptions.Count} options with the same fallback environment variable name ('{environmentVariableName}'):
{invalidOptions.JoinToString(Environment.NewLine)}

Options cannot share the same environment variable as a fallback (environment variable names are case-sensitive).";

            return new TypinException(message.Trim());
        }
    }
}