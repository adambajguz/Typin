namespace Typin.Exceptions.ArgumentBinding
{
    using System.Collections.Generic;
    using System.Linq;
    using Typin.Input;
    using Typin.Internal.Extensions;

    /// <summary>
    /// Unrecognized options exception.
    /// </summary>
    public sealed class UnrecognizedOptionsException : ArgumentBindingException
    {
        /// <summary>
        /// Initializes an instance of <see cref="UnrecognizedParametersException"/>.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="optionsInputs"></param>
        public UnrecognizedOptionsException(CommandInput input, IEnumerable<CommandOptionInput> optionsInputs) :
            base(null,
                 input,
                 BuildMessage(optionsInputs))
        {

        }

        private static string BuildMessage(IEnumerable<CommandOptionInput> optionsInputs)
        {
            string quotedParameters = optionsInputs.Select(o => o.GetRawAlias()).JoinToString(", ");

            return $"Unrecognized options provided: [{quotedParameters}]";
        }
    }
}