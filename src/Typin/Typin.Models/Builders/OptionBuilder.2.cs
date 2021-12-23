namespace Typin.Models.Builders
{
    using System;
    using System.Linq.Expressions;
    using Typin.Models.Collections;
    using Typin.Models.Converters;
    using Typin.Models.Schemas;
    using Typin.Utilities;

    /// <summary>
    /// Option builder.
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    /// <typeparam name="TProperty"></typeparam>
    internal sealed class OptionBuilder<TModel, TProperty> : ArgumentBuilder<TModel, TProperty>, IOptionBuilder<TModel, TProperty>
        where TModel : class, IModel
    {
        private string? _name;
        private char? _shortName;
        private bool _isRequired;
        private string? _description;
        private string? _fallbackVariableName;
        private Type? _converterType;

        /// <summary>
        /// Initializes a new instance of <see cref="OptionBuilder{TModel, TProperty}"/>.
        /// </summary>
        /// <param name="propertyAccessor"></param>
        public OptionBuilder(Expression<Func<TModel, TProperty>> propertyAccessor) :
            base(propertyAccessor)
        {

        }

        /// <inheritdoc/>
        public IOptionBuilder<TModel, TProperty> Name(string? name)
        {
            _name = name;

            return this;
        }

        /// <inheritdoc/>
        public IOptionBuilder<TModel, TProperty> ShortName(char? shortName)
        {
            _shortName = shortName;

            return this;
        }

        /// <inheritdoc/>
        public IOptionBuilder<TModel, TProperty> IsRequired(bool isRequired = true)
        {
            _isRequired = isRequired;

            return this;
        }

        /// <inheritdoc/>
        public IOptionBuilder<TModel, TProperty> Description(string? description)
        {
            _description = description;

            return this;
        }

        /// <inheritdoc/>
        public IOptionBuilder<TModel, TProperty> Fallback(string? variableName)
        {
            _fallbackVariableName = variableName;

            return this;
        }

        /// <inheritdoc/>
        public IOptionBuilder<TModel, TProperty> Converter<TConverter>()
            where TConverter : IArgumentConverter<TProperty>
        {
            _converterType = typeof(TConverter);

            return this;
        }

        /// <inheritdoc/>
        public IOptionBuilder<TModel, TProperty> Converter(Type? type)
        {
            if (type is not null && !IArgumentConverter<TProperty>.IsValidType(type))
            {
                throw new ArgumentException($"'{type}' is not a valid converter for option property of type '{typeof(TProperty)}' inside model '{typeof(TModel)}'.");
            }

            _converterType = type;

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

            return new OptionSchema(
                    Property,
                    optionName,
                    _shortName,
                    _fallbackVariableName,
                    _isRequired,
                    _description,
                    _converterType,
                    new MetadataCollection()
                );
        }
    }
}
