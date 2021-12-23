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
    internal class ParameterBuilder : ArgumentBuilder, IParameterBuilder
    {
        private readonly int _defaultOrder;
        private int _order;

        private string? _name;
        private string? _description;

        protected Type? ConverterType { get; set; }

        /// <summary>
        /// Initializes a new instance of <see cref="ParameterBuilder{TModel}"/>.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="defaultOrder"></param>
        /// <param name="propertyInfo"></param>
        public ParameterBuilder(Type model, int defaultOrder, PropertyInfo propertyInfo) :
            base(model, propertyInfo)
        {
            _defaultOrder = defaultOrder;
            _order = defaultOrder;
        }

        IParameterBuilder IManageExtensions<IParameterBuilder>.ManageExtensions(Action<IExtensionsCollection> action)
        {
            action(Extensions);

            return this;
        }

        /// <inheritdoc/>
        public IParameterBuilder Order(int? order)
        {
            _order = order ?? _defaultOrder;

            return this;
        }

        /// <inheritdoc/>
        public IParameterBuilder Name(string? name)
        {
            _name = name;

            return this;
        }

        /// <inheritdoc/>
        public IParameterBuilder Description(string? description)
        {
            _description = description;

            return this;
        }

        /// <inheritdoc/>
        public IParameterBuilder Converter<TConverter>()
            where TConverter : IArgumentConverter
        {
            return Converter(typeof(TConverter));
        }

        /// <inheritdoc/>
        public IParameterBuilder Converter(Type? type)
        {
            if (type is not null && !IArgumentConverter.IsValidType(type, Property.PropertyType))
            {
                throw new ArgumentException($"'{type}' is not a valid converter for parameter property of type 'Property.PropertyType' inside model '{Model}'.");
            }

            ConverterType = type;

            return this;
        }

        /// <inheritdoc/>
        public IParameterSchema Build()
        {
            // The user may mistakenly specify dashes, so trim them
            string? parameterName = _name?.TrimStart('-') ??
                TextUtils.ToKebabCase(Property.Name);

            ParameterSchema schema = new(Property,
                                          _order,
                                          parameterName,
                                          _description,
                                          ConverterType,
                                          Extensions);

            EnsureBuiltOnce(); // Call before return to allow rebuild on exception.
            return schema;
        }
    }
}
