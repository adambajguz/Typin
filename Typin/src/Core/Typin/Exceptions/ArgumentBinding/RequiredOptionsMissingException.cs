namespace Typin.Exceptions.ArgumentBinding
{
    using System.Collections.Generic;
    using System.Linq;
    using Typin.Models.Schemas;
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
        public RequiredOptionsMissingException(IEnumerable<IOptionSchema> options) :
            base(null,
                 BuildMessage(options))
        {

        }

        private static string BuildMessage(IEnumerable<IOptionSchema> options)
        {
            string quotedOptionsNames = options.Select(x => x.ToString()).JoinToString(", ");

            return $@"Missing values for one or more required options: {quotedOptionsNames}.";
        }
    }
}