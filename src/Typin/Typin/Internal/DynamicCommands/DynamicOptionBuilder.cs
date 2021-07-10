namespace Typin.DynamicCommands
{
    using System;
    using Typin.Binding;

    /// <summary>
    /// Dynamic command option builder.
    /// </summary>
    internal class DynamicOptionBuilder<T> : DynamicOptionBuilder, IDynamicOptionBuilder<T>
    {
        /// <summary>
        /// Initializes a new instace of <see cref="DynamicOptionBuilder"/>.
        /// </summary>
        public DynamicOptionBuilder() : base(typeof(T))
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

        private string? _name;
        private char? _shortName;
        private bool _isRequired;
        private string? _description;
        private string? _fallbackVariableName;
        private Type? _converter;

        /// <summary>
        /// Initializes a new instace of <see cref="DynamicOptionBuilder"/>.
        /// </summary>
        public DynamicOptionBuilder(Type type)
        {
            _type = type;
        }

        /// <inheritdoc/>
        public IDynamicOptionBuilder WithName(string? name)
        {
            _name = name;

            return this;
        }

        /// <inheritdoc/>
        public IDynamicOptionBuilder WithShortName(char? shortName)
        {
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
    }
}
