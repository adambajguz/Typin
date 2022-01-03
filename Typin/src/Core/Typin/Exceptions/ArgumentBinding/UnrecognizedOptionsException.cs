namespace Typin.Exceptions.ArgumentBinding
{
    using System.Collections.Generic;
    using System.Linq;
    using Typin.Features.Input.Tokens;
    using Typin.Utilities.Extensions;

    /// <summary>
    /// Unrecognized options exception.
    /// </summary>
    public sealed class UnrecognizedOptionsException : ArgumentBindingException
    {
        /// <summary>
        /// Initializes an instance of <see cref="UnrecognizedOptionsException"/>.
        /// </summary>
        /// <param name="optionsInputs"></param>
        public UnrecognizedOptionsException(IEnumerable<NamedToken> optionsInputs) :
            base(null,
                 BuildMessage(optionsInputs))
        {

        }

        private static string BuildMessage(IEnumerable<NamedToken> optionsInputs)
        {
            string quotedParameters = optionsInputs.Select(o => o.GetFormattedAlias()).JoinToString(", ");

            return $"Unrecognized options provided: [{quotedParameters}]";
        }
    }
}