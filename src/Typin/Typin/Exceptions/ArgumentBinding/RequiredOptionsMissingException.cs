namespace Typin.Exceptions.ArgumentBinding
{
    using System.Collections.Generic;
    using System.Linq;
    using Typin.Schemas;
    using Typin.Utilities.Extensions;

    /// <summary>
    /// Required option input missing exception.
    /// </summary>
    public sealed class RequiredOptionsMissingException : ArgumentBindingException
    {
        /// <summary>
        /// Initializes an instance of <see cref="RequiredOptionsMissingException"/>.
        /// </summary>
        /// <param name="options"></param>
        public RequiredOptionsMissingException(IEnumerable<OptionSchema> options) :
            base(null,
                 BuildMessage(options))
        {

        }

        private static string BuildMessage(IEnumerable<OptionSchema> options)
        {
            string quotedOptionsNames = options.Select(x => x.ToString()).JoinToString(", ");

            return $@"Missing values for one or more required options: {quotedOptionsNames}.";
        }
    }
}