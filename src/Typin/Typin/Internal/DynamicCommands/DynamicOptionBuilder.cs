namespace Typin.DynamicCommands
{
    using System;
    using Typin.Binding;
    using Typin.Schemas;

    /// <summary>
    /// Dynamic command option builder.
    /// </summary>
    internal class DynamicOptionBuilder<T> : DynamicOptionBuilder, IDynamicOptionBuilder<T>
    {
        /// <summary>
        /// Initializes a new instace of <see cref="DynamicOptionBuilder"/>.
        /// </summary>
        public DynamicOptionBuilder(string name) : base(name, typeof(T))
        {

        }

        /// <inheritdoc/>
        public IDynamicOptionBuilder<T> WithBindingConverter<TConverter>()
            where TConverter : BindingConverter<T>
        {
            WithBindingConverter(typeof(TConverter));

            return this;
        }
    }

    /// <summary>
    /// Dynamic command option builder.
    /// </summary>
    internal class DynamicOptionBuilder : IDynamicOptionBuilder
    {
        private readonly Type _type;
        private readonly string _name;

        private string? _propertyName;
        private char? _shortName;
        private bool _isRequired;
        private string? _description;
        private string? _fallbackVariableName;
        private Type? _converter;

        /// <summary>
        /// Initializes a new instace of <see cref="DynamicOptionBuilder"/>.
        /// </summary>
        public DynamicOptionBuilder(string name, Type type)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException($"'{nameof(name)}' cannot be null or whitespace.", nameof(name));
            }

            if (name == "help")
            {
                throw new ArgumentException("Option name 'help' is reserved.");
            }

            _name = name;
            _type = type ?? throw new ArgumentNullException(nameof(type));
        }

        /// <inheritdoc/>
        public IDynamicOptionBuilder WithPropertyName(string? propertyName)
        {
            _propertyName = string.IsNullOrWhiteSpace(propertyName) ? null : propertyName;

            return this;
        }

        /// <inheritdoc/>
        public IDynamicOptionBuilder WithShortName(char? shortName)
        {
            if (_name == "help")
            {
                throw new ArgumentException("option name 'help' is reserved.");
            }

            _shortName = shortName;

            return this;
        }

        /// <inheritdoc/>
        public IDynamicOptionBuilder AsOptional()
        {
            _isRequired = false;

            return this;
        }

        /// <inheritdoc/>
        public IDynamicOptionBuilder AsRequired()
        {
            _isRequired = true;

            return this;
        }

        /// <inheritdoc/>
        public IDynamicOptionBuilder WithRequired(bool isRequired)
        {
            _isRequired = isRequired;

            return this;
        }

        /// <inheritdoc/>
        public IDynamicOptionBuilder WithDescription(string? description)
        {
            _description = description;

            return this;
        }

        /// <inheritdoc/>
        public IDynamicOptionBuilder WithFallbackVariableName(string? variableName)
        {
            _fallbackVariableName = variableName;

            return this;
        }

        /// <inheritdoc/>
        public IDynamicOptionBuilder WithBindingConverter(Type converterType)
        {
            _converter = converterType;

            return this;
        }

        public OptionSchema Build()
        {
            return new OptionSchema(_type,
                                    _propertyName ?? _name,
                                    true,
                                    _name,
                                    _shortName,
                                    _fallbackVariableName,
                                    _isRequired,
                                    _description,
                                    _converter);
        }
    }
}
