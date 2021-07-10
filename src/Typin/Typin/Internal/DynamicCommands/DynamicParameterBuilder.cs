namespace Typin.DynamicCommands
{
    using System;
    using Typin.Binding;

    /// <summary>
    /// Dynamic command builder.
    /// </summary>
    internal class DynamicParameterBuilder<T> : DynamicParameterBuilder, IDynamicParameterBuilder<T>
    {
        /// <summary>
        /// Initializes a new instace of <see cref="DynamicParameterBuilder{Type}"/>.
        /// </summary>
        public DynamicParameterBuilder(int order) : base(typeof(T), order)
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
        private readonly int _order;

        private string? _name;
        private string? _description;
        private Type? _converter;

        /// <summary>
        /// Initializes a new instace of <see cref="DynamicParameterBuilder"/>.
        /// </summary>
        public DynamicParameterBuilder(Type type, int order)
        {
            _type = type;
            _order = order;
        }

        /// <inheritdoc/>
        public IDynamicParameterBuilder WithName(string? name)
        {
            _name = name;

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
    }
}
