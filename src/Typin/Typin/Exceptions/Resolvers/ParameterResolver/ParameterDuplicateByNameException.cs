namespace Typin.Exceptions.Resolvers.ParameterResolver
{
    using System;
    using System.Collections.Generic;
    using Typin.Schemas;
    using Typin.Utilities.Extensions;

    /// <summary>
    /// Parameter duplicate by name exception.
    /// </summary>
    public sealed class ParameterDuplicateByNameException : ParameterResolverException
    {
        /// <summary>
        /// Duplicated parameter name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Initializes an instance of <see cref="ParameterDuplicateByNameException"/>.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="name"></param>
        /// <param name="invalidParameters"></param>
        public ParameterDuplicateByNameException(BaseCommandSchema command, string name, IReadOnlyList<ParameterSchema> invalidParameters) :
            base(command,
                 invalidParameters,
                 BuildMessage(command, name, invalidParameters))
        {
            Name = name;
        }

        private static string BuildMessage(BaseCommandSchema command, string name, IReadOnlyList<ParameterSchema> invalidParameters)
        {
            return $"Command '{command.Type.FullName}' is invalid because it contains {invalidParameters.Count} parameters with the same name ('{name}'):{Environment.NewLine}" +
                   $"{invalidParameters.JoinToString(Environment.NewLine)}{Environment.NewLine}" +
                   Environment.NewLine +
                   "Parameters must have unique names (names are case-sensitive).";
        }
    }
}