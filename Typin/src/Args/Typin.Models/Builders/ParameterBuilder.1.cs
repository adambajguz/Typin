namespace Typin.Models.Builders
{
    using System;
    using System.Reflection;
    using Typin.Schemas.Builders;
    using Typin.Schemas.Collections;

    /// <summary>
    /// Prameter builder.
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    internal class ParameterBuilder<TModel> : ParameterBuilder, IParameterBuilder<TModel>
        where TModel : class, IModel
    {
        IParameterBuilder<TModel> ISelf<IParameterBuilder<TModel>>.Self => this;

        /// <summary>
        /// Initializes a new instance of <see cref="ParameterBuilder{TModel}"/>.
        /// </summary>
        /// <param name="defaultOrder"></param>
        /// <param name="propertyInfo"></param>
        public ParameterBuilder(int defaultOrder, PropertyInfo propertyInfo) :
            base(typeof(TModel), defaultOrder, propertyInfo)
        {

        }

        IParameterBuilder<TModel> IManageExtensions<IParameterBuilder<TModel>>.ManageExtensions(Action<IExtensionsCollection> action)
        {
            action(Extensions);

            return this;
        }

        IParameterBuilder<TModel> IParameterBuilder<TModel>.Order(int? order)
        {
            Order(order);

            return this;
        }

        IParameterBuilder<TModel> IParameterBuilder<TModel>.Name(string? name)
        {
            Name(name);

            return this;
        }

        IParameterBuilder<TModel> IParameterBuilder<TModel>.Description(string? description)
        {
            Description(description);

            return this;
        }

        IParameterBuilder<TModel> IParameterBuilder<TModel>.Converter<TConverter>()
        {
            Converter(typeof(TConverter));

            return this;
        }

        IParameterBuilder<TModel> IParameterBuilder<TModel>.Converter(Type? type)
        {
            Converter(type);

            return this;
        }
    }
}
