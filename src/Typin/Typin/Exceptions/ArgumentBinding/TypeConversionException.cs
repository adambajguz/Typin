namespace Typin.Exceptions.ArgumentBinding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Typin.Models.Schemas;

    /// <summary>
    /// Type conversion exception.
    /// </summary>
    public sealed class TypeConversionException : ArgumentBindingException
    {
        /// <summary>
        /// Values.
        /// </summary>
        public IReadOnlyCollection<string?> Values { get; }

        /// <summary>
        /// Target type.
        /// </summary>
        public Type TargetType { get; }

        /// <summary>
        /// Initializes an instance of <see cref="TypeConversionException"/>.
        /// </summary>
        /// <param name="argument"></param>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="innerException"></param>
        public TypeConversionException(IArgumentSchema argument, string? value, Type targetType, Exception? innerException = null) :
            this(argument, new[] { value }, targetType, innerException)
        {

        }

        /// <summary>
        /// Initializes an instance of <see cref="TypeConversionException"/>.
        /// </summary>
        /// <param name="argument"></param>
        /// <param name="values"></param>
        /// <param name="targetType"></param>
        /// <param name="innerException"></param>
        public TypeConversionException(IArgumentSchema argument, IReadOnlyCollection<string?> values, Type targetType, Exception? innerException = null) :
            base(argument,
                 BuildMessage(argument, values, targetType, innerException),
                 innerException)
        {
            Values = values;
            TargetType = targetType;
        }

        private static string BuildMessage(IArgumentSchema argument, IReadOnlyCollection<string?> values, Type targetType, Exception? innerException)
        {
            string argumentKind = argument switch
            {
                ParameterSchema => "Parameter",
                OptionSchema => "Option",
                _ => "Argument"
            };

            StringBuilder builder = new();
            builder.Append("Cannot convert value");

            if (argument.Bindable.IsScalar)
            {
                builder.Append(" \"");
                builder.Append(values.FirstOrDefault() ?? "<null>");
                builder.Append('"');
            }
            else
            {
                builder.Append("s [");
                builder.AppendJoin(", \"", values);
                builder.Append(']');
            }

            builder.Append(" to type '");
            builder.Append(targetType.Name);
            builder.Append("' for ");
            builder.Append(argumentKind);
            builder.Append('\'');
            builder.Append(argument);
            builder.AppendLine("'.");
            builder.Append(innerException?.Message ?? "This type is not supported.");

            return builder.ToString();
        }
    }
}