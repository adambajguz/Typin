namespace Typin.Models.Builders
{
    using System;
    using System.Reflection;
    using Typin.Models.Converters;
    using Typin.Models.Schemas;
    using Typin.Schemas.Collections;
    using Typin.Utilities;

    /// <summary>
    /// Option builder.
    /// </summary>
    internal class OptionBuilder : ArgumentBuilder, IOptionBuilder
    {
        private string? _name;
        private char? _shortName;
        private bool _isRequired;
        private string? _description;
        private string? _fallbackVariableName;

        protected Type? ConverterType { get; set; }

        /// <summary>
        /// Initializes a new instance of <see cref="OptionBuilder"/>.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="propertyInfo"></param>
        public OptionBuilder(Type model, PropertyInfo propertyInfo) :
            base(model, propertyInfo)
        {

        }

        IOptionBuilder IManageExtensions<IOptionBuilder>.ManageExtensions(Action<IExtensionsCollection> action)
        {
            action(Extensions);

            return this;
        }

        /// <inheritdoc/>
        public IOptionBuilder Name(string? name)
        {
            _name = name;

            return this;
        }

        /// <inheritdoc/>
        public IOptionBuilder ShortName(char? shortName)
        {
            _shortName = shortName;

            return this;
        }

        /// <inheritdoc/>
        public IOptionBuilder IsRequired(bool isRequired = true)
        {
            _isRequired = isRequired;

            return this;
        }

        /// <inheritdoc/>
        public IOptionBuilder Description(string? description)
        {
            _description = description;

            return this;
        }

        /// <inheritdoc/>
        public IOptionBuilder Fallback(string? variableName)
        {
            _fallbackVariableName = variableName;

            return this;
        }

        /// <inheritdoc/>
        public IOptionBuilder Converter<TConverter>()
            where TConverter : IArgumentConverter
        {
            return Converter(typeof(TConverter));
        }

        /// <inheritdoc/>
        public IOptionBuilder Converter(Type? type)
        {
            if (type is not null && !IArgumentConverter.IsValidType(type, Property.PropertyType))
            {
                throw new ArgumentException($"'{type}' is not a valid converter for option property of type '{Property.PropertyType}' inside model '{Model}'.");
            }

            ConverterType = type;

            return this;
        }

        /// <inheritdoc/>
        public IOptionSchema Build()
        {
            // The user may mistakenly specify dashes, thinking it's required, so trim them
            string? optionName = _name?.TrimStart('-');

            if (optionName is null && _shortName is null)
            {
                _name = TextUtils.ToKebabCase(Property.Name);
            }
            else
            {
                if (optionName is string n && (n.Contains(' ') || !IOptionSchema.IsName("--" + n)))
                {
                    string message = $@"Command option name '{optionName}' is invalid.

Options must have a name starting from letter, while short names must be a letter.

Option names must be at least 2 characters long to avoid confusion with short names.
If you intended to set the short name instead, use the attribute overload that accepts a char.";

                    throw new InvalidOperationException(message.Trim());
                }

                if (_shortName is char sn && !IOptionSchema.IsShortName("-" + sn))
                {
                    string message = $"Command option short name '{_shortName}' is invalid. Options must have a name starting from letter, while short names must be a letter.";
                    throw new InvalidOperationException(message.Trim());
                }
            }

            OptionSchema schema = new(Property,
                                      optionName,
                                      _shortName,
                                      _fallbackVariableName,
                                      _isRequired,
                                      _description,
                                      ConverterType,
                                      Extensions);

            EnsureBuiltOnce(); // Call before return to allow rebuild on exception.
            return schema;
        }
    }
}
