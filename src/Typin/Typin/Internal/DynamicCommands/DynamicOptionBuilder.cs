namespace Typin.Internal.DynamicCommands
{
    using System;
    using System.Collections.Generic;
    using Typin.DynamicCommands;
    using Typin.Models.Collections;
    using Typin.Models.Converters;
    using Typin.Models.Schemas;
    using Typin.Utilities;

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
            where TConverter : IArgumentConverter<T>
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
        private readonly string _propertyName;

        private string? _name;
        private char? _shortName;
        private bool _isRequired;
        private string? _description;
        private string? _fallbackVariableName;
        private Type? _converter;
        private readonly Dictionary<Type, IArgumentMetadata> _metadata = new();

        /// <summary>
        /// Initializes a new instace of <see cref="DynamicOptionBuilder"/>.
        /// </summary>
        public DynamicOptionBuilder(string propertyName, Type type)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
            {
                throw new ArgumentException($"'{nameof(propertyName)}' cannot be null or whitespace.", nameof(propertyName));
            }

            if (propertyName == "help")
            {
                throw new ArgumentException("Option name 'help' is reserved.");
            }

            _propertyName = propertyName;
            _type = type ?? throw new ArgumentNullException(nameof(type));
        }

        /// <inheritdoc/>
        public IDynamicOptionBuilder WithName(string? name)
        {
            _name = string.IsNullOrWhiteSpace(name) ? null : name;

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

        /// <inheritdoc/>

        public IDynamicOptionBuilder SetMetadata(IArgumentMetadata metadata)
        {
            _metadata[metadata.GetType()] = metadata;

            return this;
        }

        public IOptionSchema Build()
        {
            return new OptionSchema(_type,
                                    _propertyName,
                                    _name ?? TextUtils.ToKebabCase(_propertyName),
                                    _shortName,
                                    _fallbackVariableName,
                                    _isRequired,
                                    _description,
                                    _converter,
                                    new MetadataCollection(_metadata));
        }
    }
}
