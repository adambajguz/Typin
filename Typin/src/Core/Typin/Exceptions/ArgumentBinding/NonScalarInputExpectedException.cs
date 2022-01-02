namespace Typin.Exceptions.ArgumentBinding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Typin.Models.Schemas;
    using Typin.Utilities.Extensions;

    /// <summary>
    /// Non-scalar input expected exception.
    /// </summary>
    public sealed class NonScalarInputExpectedException : ArgumentBindingException
    {
        /// <summary>
        /// Values.
        /// </summary>
        public IReadOnlyCollection<string> Values { get; }

        /// <summary>
        /// Initializes an instance of <see cref="NonScalarInputExpectedException"/>.
        /// </summary>
        /// <param name="argument"></param>
        /// <param name="values"></param>
        public NonScalarInputExpectedException(IArgumentSchema argument, IReadOnlyCollection<string> values) :
            base(argument,
                 BuildMessage(argument, values))
        {
            Values = values;
        }

        private static string BuildMessage(IArgumentSchema argument, IReadOnlyCollection<string> values)
        {
            string argumentKind = argument switch
            {
                ParameterSchema => "Parameter",
                OptionSchema => "Option",
                _ => "Argument"
            };

            string quotedValues = values.Select(v => v.Quote()).JoinToString(' ');

            return $"{argumentKind} '{argument}' expects a single value (non-scalar) input, but multiple values provided:{Environment.NewLine}{quotedValues}";
        }
    }
}