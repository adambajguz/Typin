namespace Typin.Models.Builders
{
    using System;
    using System.Linq.Expressions;
    using Typin.Models.Collections;
    using Typin.Models.Converters;
    using Typin.Models.Schemas;
    using Typin.Utilities;

    /// <summary>
    /// Prameter builder.
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    /// <typeparam name="TProperty"></typeparam>
    internal sealed class ParameterBuilder<TModel, TProperty> : ArgumentBuilder<TModel, TProperty>, IParameterBuilder<TModel, TProperty>
        where TModel : class, IModel
    {
        private readonly int _defaultOrder;
        private int _order;

        private string? _name;
        private string? _description;
        private Type? _converterType;

        /// <summary>
        /// Initializes a new instance of <see cref="ParameterBuilder{TModel, TProperty}"/>.
        /// </summary>
        /// <param name="defaultOrder"></param>
        /// <param name="propertyAccessor"></param>
        public ParameterBuilder(int defaultOrder, Expression<Func<TModel, TProperty>> propertyAccessor) :
            base(propertyAccessor)
        {
            _defaultOrder = defaultOrder;
            _order = defaultOrder;
        }

        /// <inheritdoc/>
        public IParameterBuilder<TModel, TProperty> Order(int? order)
        {
            _order = order ?? _defaultOrder;

            return this;
        }

        /// <inheritdoc/>
        public IParameterBuilder<TModel, TProperty> Name(string? name)
        {
            _name = name;

            return this;
        }

        /// <inheritdoc/>
        public IParameterBuilder<TModel, TProperty> Description(string? description)
        {
            _description = description;

            return this;
        }

        /// <inheritdoc/>
        public IParameterBuilder<TModel, TProperty> Converter<TConverter>()
            where TConverter : IArgumentConverter<TProperty>
        {
            _converterType = typeof(TConverter);

            return this;
        }

        /// <inheritdoc/>
        public IParameterBuilder<TModel, TProperty> Converter(Type? type)
        {
            if (type is not null && !IArgumentConverter<TProperty>.IsValidType(type))
            {
                throw new ArgumentException($"'{type}' is not a valid converter for parameter property of type '{typeof(TProperty)}' inside model '{typeof(TModel)}'.");
            }

            _converterType = type;

            return this;
        }

        /// <inheritdoc/>
        public IParameterSchema Build()
        {
            // The user may mistakenly specify dashes, so trim them
            string? parameterName = _name?.TrimStart('-') ??
                TextUtils.ToKebabCase(Property.Name);

            return new ParameterSchema(
                    Property,
                    _order,
                    parameterName,
                    _description,
                    _converterType,
                    new MetadataCollection()
                );
        }
    }
}
