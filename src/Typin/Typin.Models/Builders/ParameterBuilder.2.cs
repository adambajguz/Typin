namespace Typin.Models.Builders
{
    using System;
    using System.Linq.Expressions;
    using Typin.Models.Collections;
    using Typin.Models.Converters;
    using Typin.Models.Extensions;

    /// <summary>
    /// Prameter builder.
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    /// <typeparam name="TProperty"></typeparam>
    internal class ParameterBuilder<TModel, TProperty> : ParameterBuilder<TModel>, IParameterBuilder<TModel, TProperty>
        where TModel : class, IModel
    {
        /// <summary>
        /// Initializes a new instance of <see cref="ParameterBuilder{TModel, TProperty}"/>.
        /// </summary>
        /// <param name="defaultOrder"></param>
        /// <param name="propertyAccessor"></param>
        public ParameterBuilder(int defaultOrder, Expression<Func<TModel, TProperty>> propertyAccessor) :
            base(defaultOrder, propertyAccessor.GetPropertyInfo())
        {

        }

        IParameterBuilder<TModel, TProperty> IManageExtensions<IParameterBuilder<TModel, TProperty>>.ManageExtensions(Action<IExtensionsCollection> action)
        {
            action(Extensions);

            return this;
        }

        IParameterBuilder<TModel, TProperty> IParameterBuilder<TModel, TProperty>.Order(int? order)
        {
            Order(order);

            return this;
        }

        IParameterBuilder<TModel, TProperty> IParameterBuilder<TModel, TProperty>.Name(string? name)
        {
            Name(name);

            return this;
        }

        IParameterBuilder<TModel, TProperty> IParameterBuilder<TModel, TProperty>.Description(string? description)
        {
            Description(description);

            return this;
        }

        IParameterBuilder<TModel, TProperty> IParameterBuilder<TModel, TProperty>.Converter<TConverter>()
        {
            ConverterType = typeof(TConverter);

            return this;
        }

        IParameterBuilder<TModel, TProperty> IParameterBuilder<TModel, TProperty>.Converter(Type? type)
        {
            if (type is not null && !IArgumentConverter<TProperty>.IsValidType(type))
            {
                throw new ArgumentException($"'{type}' is not a valid converter for parameter property of type '{typeof(TProperty)}' inside model '{typeof(TModel)}'.");
            }

            ConverterType = type;

            return this;
        }
    }
}
