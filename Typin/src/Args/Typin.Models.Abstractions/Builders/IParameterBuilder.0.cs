namespace Typin.Models.Builders
{
    using System;
    using Typin.Models.Converters;
    using Typin.Models.Schemas;

    /// <summary>
    /// Model parameter builder.
    /// </summary>
    public interface IParameterBuilder : IBuilder<IParameterSchema>, IManageExtensions<IParameterBuilder>
    {
        /// <summary>
        /// Sets parameter order to default (parameter registration order).
        ///
        /// All parameters in a command must have different order.
        /// Parameter whose type is a non-scalar (e.g. array), must be the last in order and only one such parameter is allowed.
        /// </summary>
        /// <returns></returns>
        public IParameterBuilder DefaultOrder()
        {
            return Order(null);
        }

        /// <summary>
        /// Sets parameter name to default (kebab-cased property name).
        /// </summary>
        /// <returns></returns>
        public IParameterBuilder DefaultName()
        {
            return Name(null);
        }

        /// <summary>
        /// Sets parameter description to default (no description).
        /// </summary>
        /// <returns></returns>
        public IParameterBuilder DefaultDescription()
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
        IParameterBuilder Order(int? order);

        /// <summary>
        /// Sets parameter name.
        /// When null is passed a value will be set to default (kebab-cased property name).
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        IParameterBuilder Name(string? name);

        /// <summary>
        /// Sets parameter description.
        /// When null is passed a value will be set to default (no description).
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        IParameterBuilder Description(string? description);

        /// <summary>
        /// Sets a parameter converter.
        /// </summary>
        /// <typeparam name="TConverter"></typeparam>
        /// <returns></returns>
        IParameterBuilder Converter<TConverter>()
            where TConverter : IArgumentConverter;

        /// <summary>
        /// Sets an parameter converter.
        /// When null is passed a value will be set to default (no converter).
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        IParameterBuilder Converter(Type? type);
    }
}