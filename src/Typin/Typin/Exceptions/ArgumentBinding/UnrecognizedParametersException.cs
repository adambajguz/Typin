namespace Typin.Exceptions.ArgumentBinding
{
    using System.Collections.Generic;
    using System.Linq;
    using Typin.Input;
    using Typin.Utilities.Extensions;

    /// <summary>
    /// Unrecognized parameters exception.
    /// </summary>
    public sealed class UnrecognizedParametersException : ArgumentBindingException
    {
        /// <summary>
        /// Initializes an instance of <see cref="UnrecognizedParametersException"/>.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="parameterInputs"></param>
        public UnrecognizedParametersException(ParsedCommandInput input, IEnumerable<CommandParameterInput> parameterInputs) :
            base(null,
                 input,
                 BuildMessage(parameterInputs))
        {

        }

        private static string BuildMessage(IEnumerable<CommandParameterInput> parameterInputs)
        {
            string quotedParameters = parameterInputs.Select(p => p.Value).JoinToString(", ");

            return $"Unrecognized parameters provided: [{quotedParameters}]";
        }
    }
}