namespace Typin.Models.Builders
{
    using System;
    using System.Reflection;
    using Typin.Models.Collections;
    using Typin.Models.Converters;
    using Typin.Models.Schemas;
    using Typin.Utilities;

    /// <summary>
    /// Prameter builder.
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    internal sealed class ParameterBuilder<TModel> : ArgumentBuilder<TModel>, IParameterBuilder<TModel>
        where TModel : class, IModel
    {
        private readonly int _defaultOrder;
        private int _order;

        private string? _name;
        private string? _description;
        private Type? _converterType;

        /// <summary>
        /// Initializes a new instance of <see cref="ParameterBuilder{TModel}"/>.
        /// </summary>
        /// <param name="defaultOrder"></param>
        /// <param name="propertyInfo"></param>
        public ParameterBuilder(int defaultOrder, PropertyInfo propertyInfo) :
            base(propertyInfo)
        {
            _defaultOrder = defaultOrder;
            _order = defaultOrder;
        }

        /// <inheritdoc/>
        public IParameterBuilder<TModel> Order(int? order)
        {
            _order = order ?? _defaultOrder;

            return this;
        }

        /// <inheritdoc/>
        public IParameterBuilder<TModel> Name(string? name)
        {
            _name = name;

            return this;
        }

        /// <inheritdoc/>
        public IParameterBuilder<TModel> Description(string? description)
        {
            _description = description;

            return this;
        }

        /// <inheritdoc/>
        public IParameterBuilder<TModel> Converter<TConverter>()
            where TConverter : IArgumentConverter
        {
            return Converter(typeof(TConverter));
        }

        /// <inheritdoc/>
        public IParameterBuilder<TModel> Converter(Type? type)
        {
            if (type is not null && !IArgumentConverter.IsValidType(type, Property.PropertyType))
            {
                throw new ArgumentException($"'{type}' is not a valid converter for parameter property of type 'Property.PropertyType' inside model '{typeof(TModel)}'.");
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
