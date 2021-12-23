namespace Typin.Exceptions.ArgumentBinding
{
    using System.Collections.Generic;
    using System.Linq;
    using Typin.Features.Input;
    using Typin.Utilities.Extensions;

    /// <summary>
    /// Unrecognized options exception.
    /// </summary>
    public sealed class UnrecognizedOptionsException : ArgumentBindingException
    {
        /// <summary>
        /// Initializes an instance of <see cref="UnrecognizedParametersException"/>.
        /// </summary>
        /// <param name="optionsInputs"></param>
        public UnrecognizedOptionsException(IEnumerable<OptionInput> optionsInputs) :
            base(null,
                 BuildMessage(optionsInputs))
        {

        }

        private static string BuildMessage(IEnumerable<OptionInput> optionsInputs)
        {
            string quotedParameters = optionsInputs.Select(o => o.GetRawAlias()).JoinToString(", ");

            return $"Unrecognized options provided: [{quotedParameters}]";
        }
    }
}