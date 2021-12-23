namespace Typin.Models.Builders
{
    using System;
    using Typin.Models;
    using Typin.Models.Converters;
    using Typin.Models.Schemas;

    /// <summary>
    /// Model parameter builder.
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    /// <typeparam name="TProperty"></typeparam>
    public interface IParameterBuilder<TModel, TProperty> : IBuilder<IParameterSchema>
        where TModel : class, IModel
    {
        /// <summary>
        /// Sets parameter order to default (parameter registration order).
        ///
        /// All parameters in a command must have different order.
        /// Parameter whose type is a non-scalar (e.g. array), must be the last in order and only one such parameter is allowed.
        /// </summary>
        /// <returns></returns>
        public IParameterBuilder<TModel, TProperty> DefaultOrder()
        {
            return Order(null);
        }

        /// <summary>
        /// Sets parameter name to default (kebab-cased property name).
        /// </summary>
        /// <returns></returns>
        public IParameterBuilder<TModel, TProperty> DefaultName()
        {
            return Name(null);
        }

        /// <summary>
        /// Sets parameter description to default (no description).
        /// </summary>
        /// <returns></returns>
        public IParameterBuilder<TModel, TProperty> DefaultDescription()
        {
            return Description(null);
        }

        /// <summary>
        /// Sets parameter order.
        ///
        /// All parameters in a command must have different order.
        /// Parameter whose type is a non-scalar (e.g. array), must be the last in order and only one such parameter is allowed.
        ///
        /// When null is passed a value will be set to default (parameter registration order).
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        IParameterBuilder<TModel, TProperty> Order(int? order);

        /// <summary>
        /// Sets parameter name.
        /// When null is passed a value will be set to default (kebab-cased property name).
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        IParameterBuilder<TModel, TProperty> Name(string? name);

        /// <summary>
        /// Sets parameter name.
        /// When null is passed a value will be set to default (no description).
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        IParameterBuilder<TModel, TProperty> Description(string? description);

        /// <summary>
        /// Sets a parameter converter.
        /// </summary>
        /// <typeparam name="TConverter"></typeparam>
        /// <returns></returns>
        IParameterBuilder<TModel, TProperty> Converter<TConverter>()
            where TConverter : IArgumentConverter<TProperty>;

        /// <summary>
        /// Sets an parameter converter.
        /// When null is passed a value will be set to default (no converter).
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        IParameterBuilder<TModel, TProperty> Converter(Type? type);
    }
}