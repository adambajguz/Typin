namespace Typin.Exceptions.Resolvers.ParameterResolver
{
    using System;
    using System.Collections.Generic;
    using Typin.Models.Binding;
    using Typin.Models.Schemas;
    using Typin.Utilities.Extensions;

    /// <summary>
    /// Too many non-scalar parameters exception.
    /// </summary>
    public sealed class TooManyNonScalarParametersException : ParameterResolverException
    {
        /// <summary>
        /// Initializes an instance of <see cref="ParameterDuplicateByNameException"/>.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="invalidParameters"></param>
        public TooManyNonScalarParametersException(IModelSchema command, IReadOnlyList<IParameterSchema> invalidParameters) :
            base(command,
                 invalidParameters,
                 BuildMessage(command, invalidParameters))
        {

        }

        private static string BuildMessage(IModelSchema command, IReadOnlyList<IParameterSchema> invalidParameters)
        {
            return $"Command '{command}' is invalid because it contains {invalidParameters.Count} non-scalar parameters:" +
$"{invalidParameters.JoinToString(Environment.NewLine)}" +
Environment.NewLine +
"Non-scalar parameter is such that is bound from more than one value (e.g. array)." +
Environment.NewLine +
"Only one parameter in a command may be non-scalar and it must be the last one in order." +
Environment.NewLine +
Environment.NewLine +
"If it's not feasible to fit into these constraints, consider using options instead as they don't have these limitations.";
        }
    }
}