namespace Typin.Internal.DynamicCommands
{
    using System;
    using System.Collections.Generic;
    using Typin.Binding;
    using Typin.DynamicCommands;
    using Typin.Metadata;
    using Typin.Schemas;
    using Typin.Utilities;

    /// <summary>
    /// Dynamic command builder.
    /// </summary>
    internal class DynamicParameterBuilder<T> : DynamicParameterBuilder, IDynamicParameterBuilder<T>
    {
        /// <summary>
        /// Initializes a new instace of <see cref="DynamicParameterBuilder{Type}"/>.
        /// </summary>
        public DynamicParameterBuilder(string propertyName, int order) : base(typeof(T), propertyName, order)
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
        private readonly string _propertyName;
        private readonly int _order;

        private string? _name;
        private string? _description;
        private Type? _converter;
        private readonly Dictionary<Type, IArgumentMetadata> _metadata = new();

        /// <summary>
        /// Initializes a new instace of <see cref="DynamicParameterBuilder"/>.
        /// </summary>
        public DynamicParameterBuilder(Type type, string propertyName, int order)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
            {
                throw new ArgumentException($"'{nameof(propertyName)}' cannot be null or whitespace.", nameof(propertyName));
            }

            _type = type ?? throw new ArgumentNullException(nameof(type));
            _propertyName = propertyName;
            _order = order;
        }

        /// <inheritdoc/>
        public IDynamicParameterBuilder WithName(string? name)
        {
            _name = string.IsNullOrWhiteSpace(name) ? null : name;

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

        /// <inheritdoc/>

        public IDynamicParameterBuilder WithMetadata(IArgumentMetadata metadata)
        {
            _metadata[metadata.GetType()] = metadata;

            return this;
        }

        public ParameterSchema Build()
        {
            return new ParameterSchema(_type,
                                       _propertyName,
                                       _order,
                                       _name ?? TextUtils.ToKebabCase(_propertyName),
                                       _description,
                                       _converter,
                                       new MetadataCollection(_metadata));
        }
    }
}
