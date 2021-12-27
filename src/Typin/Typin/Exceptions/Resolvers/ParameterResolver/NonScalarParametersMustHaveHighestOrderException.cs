namespace Typin.Exceptions.Resolvers.ParameterResolver
{
    using System;
    using Typin.Models.Schemas;

    /// <summary>
    /// Non-scalar parameter must have highest order exception.
    /// </summary>
    public sealed class NonScalarParametersMustHaveHighestOrderException : ParameterResolverException
    {
        /// <summary>
        /// Initializes an instance of <see cref="NonScalarParametersMustHaveHighestOrderException"/>.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="invalidParameter"></param>
        public NonScalarParametersMustHaveHighestOrderException(IModelSchema command, IParameterSchema invalidParameter) :
            base(command,
                 new[] { invalidParameter },
                 BuildMessage(command, invalidParameter))
        {

        }

        private static string BuildMessage(IModelSchema command, IParameterSchema invalidParameter)
        {
            return $"Command '{command.Type.FullName}' is invalid because it contains a non-scalar parameter {invalidParameter.Name} that is not the last one in order:{Environment.NewLine}" +
                   $"Non-scalar parameter is such that is bound from more than one value (e.g. array).{Environment.NewLine}" +
                   $"Only one parameter in a command may be non-scalar and it must be the last one in order.{Environment.NewLine}" +
                   Environment.NewLine +
                   "If it's not feasible to fit into these constraints, consider using options instead as they don't have these limitations.";
        }
    }
}