namespace Typin.DynamicCommands
{
    using System;
    using Typin.Binding;
    using Typin.Schemas;

    /// <summary>
    /// Dynamic command builder.
    /// </summary>
    internal class DynamicParameterBuilder<T> : DynamicParameterBuilder, IDynamicParameterBuilder<T>
    {
        /// <summary>
        /// Initializes a new instace of <see cref="DynamicParameterBuilder{Type}"/>.
        /// </summary>
        public DynamicParameterBuilder(string name, int order) : base(typeof(T), name, order)
        {

        }

        /// <inheritdoc/>
        public IDynamicParameterBuilder<T> WithBindingConverter<TConverter>()
            where TConverter : BindingConverter<T>
        {
            WithBindingConverter(typeof(TConverter));

            return this;
        }
    }

    /// <summary>
    /// Dynamic command builder.
    /// </summary>
    internal class DynamicParameterBuilder : IDynamicParameterBuilder
    {
        private readonly Type _type;
        private readonly string _name;
        private readonly int _order;

        private string? _propertyName;
        private string? _description;
        private Type? _converter;

        /// <summary>
        /// Initializes a new instace of <see cref="DynamicParameterBuilder"/>.
        /// </summary>
        public DynamicParameterBuilder(Type type, string name, int order)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException($"'{nameof(name)}' cannot be null or whitespace.", nameof(name));
            }

            _type = type ?? throw new ArgumentNullException(nameof(type));
            _name = name;
            _order = order;
        }

        /// <inheritdoc/>
        public IDynamicParameterBuilder WithPropertyName(string? propertyName)
        {
            _propertyName = string.IsNullOrWhiteSpace(propertyName) ? null : propertyName;

            return this;
        }

        /// <inheritdoc/>
        public IDynamicParameterBuilder WithDescription(string? description)
        {
            _description = description;

            return this;
        }

        /// <inheritdoc/>
        public IDynamicParameterBuilder WithBindingConverter(Type converterType)
        {
            _converter = converterType;

            return this;
        }

        public ParameterSchema Build()
        {
            return new ParameterSchema(_type,
                                       _propertyName ?? _name,
                                       _order,
                                       _name,
                                       _description,
                                       _converter);
        }
    }
}
