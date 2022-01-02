namespace Typin.Exceptions.ArgumentBinding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Typin.Models.Schemas;
    using Typin.Utilities.Extensions;

    /// <summary>
    /// Scalar input non-constructible exception.
    /// </summary>
    public sealed class NonScalarNonConstructibleFromArrayException : ArgumentBindingException
    {
        /// <summary>
        /// Values.
        /// </summary>
        public IReadOnlyCollection<string> Values { get; }

        /// <summary>
        /// Initializes an instance of <see cref="NonScalarNonConstructibleFromArrayException"/>.
        /// </summary>
        /// <param name="argument"></param>
        /// <param name="values"></param>
        /// <param name="targetType"></param>
        public NonScalarNonConstructibleFromArrayException(IArgumentSchema argument, IReadOnlyCollection<string> values, Type targetType) :
            base(argument,
                 BuildMessage(argument, values, targetType))
        {
            Values = values;
        }

        private static string BuildMessage(IArgumentSchema argument, IReadOnlyCollection<string> values, Type targetType)
        {
            string argumentKind = argument switch
            {
                ParameterSchema => "Parameter",
                OptionSchema => "Option",
                _ => "Argument"
            };

            string quotedValues = values.Select(v => v.Quote()).JoinToString(' ');

            return $"Can't convert provided values to type '{targetType.Name}' for {argumentKind} '{argument}':{Environment.NewLine}{quotedValues}" +
                   $"{Environment.NewLine}" +
                   $"{Environment.NewLine}" +
                   $"Target type is not assignable from array and doesn't have a public constructor that takes an array.";
        }
    }
}