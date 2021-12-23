namespace Typin.Exceptions.ArgumentBinding
{
    using System.Collections.Generic;
    using System.Linq;
    using Typin.Features.Input;
    using Typin.Utilities.Extensions;

    /// <summary>
    /// Unrecognized parameters exception.
    /// </summary>
    public sealed class UnrecognizedParametersException : ArgumentBindingException
    {
        /// <summary>
        /// Initializes an instance of <see cref="UnrecognizedParametersException"/>.
        /// </summary>
        /// <param name="parameterInputs"></param>
        public UnrecognizedParametersException(IEnumerable<ParameterInput> parameterInputs) :
            base(null,
                 BuildMessage(parameterInputs))
        {

        }

        private static string BuildMessage(IEnumerable<ParameterInput> parameterInputs)
        {
            string quotedParameters = parameterInputs.Select(p => p.Value).JoinToString(", ");

            return $"Unrecognized parameters provided: [{quotedParameters}]";
        }
    }
}