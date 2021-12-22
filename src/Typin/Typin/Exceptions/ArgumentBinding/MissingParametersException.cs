﻿namespace Typin.Exceptions.ArgumentBinding
{
    using System.Collections.Generic;
    using System.Linq;
    using Typin.Schemas;
    using Typin.Utilities.Extensions;

    /// <summary>
    /// Missing parmeters input exception.
    /// </summary>
    public sealed class MissingParametersException : ArgumentBindingException
    {
        /// <summary>
        /// Initializes an instance of <see cref="MissingParametersException"/>.
        /// </summary>
        /// <param name="parameter"></param>
        public MissingParametersException(ParameterSchema parameter) :
            base(null,
                 BuildMessage(parameter))
        {

        }

        /// <summary>
        /// Initializes an instance of <see cref="MissingParametersException"/>.
        /// </summary>
        /// <param name="parameters"></param>
        public MissingParametersException(IEnumerable<ParameterSchema> parameters) :
            base(null,
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