namespace Typin.Exceptions.ArgumentBinding
{
    using System.Collections.Generic;
    using System.Linq;
    using Typin.Input;
    using Typin.Internal.Extensions;
    using Typin.Schemas;

    /// <summary>
    /// Missing parmeters input exception.
    /// </summary>
    public sealed class MissingParametersException : ArgumentBindingException
    {
        /// <summary>
        /// Initializes an instance of <see cref="MissingParametersException"/>.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="parameter"></param>
        public MissingParametersException(CommandInput input, ParameterSchema parameter) :
            base(null,
                 input,
                 BuildMessage(parameter))
        {

        }

        /// <summary>
        /// Initializes an instance of <see cref="MissingParametersException"/>.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="parameters"></param>
        public MissingParametersException(CommandInput input, IEnumerable<ParameterSchema> parameters) :
            base(null,
                 input,
                 BuildMessage(parameters))
        {

        }

        private static string BuildMessage(ParameterSchema parameter)
        {
            return $@"Missing value for parameter '{parameter}'.";
        }

        private static string BuildMessage(IEnumerable<ParameterSchema> parameters)
        {
            string quotedParmeterNames = parameters.Select(x => x.ToString()).JoinToString(", ");

            return $@"Missing value for parameters: {quotedParmeterNames}.";
        }
    }
}