namespace Typin.Models.Builders
{
    using System;
    using System.Linq.Expressions;
    using System.Reflection;
    using Typin.Models.Binding;

    /// <summary>
    /// Model builder.
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    public interface IModelBuilder<TModel> : IBuilder<IModelSchema>, IManageExtensions<IModelBuilder<TModel>>
        where TModel : class, IModel
    {
        /// <summary>
        /// Configures an parameter property.
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="property"></param>
        /// <returns></returns>
        IParameterBuilder<TModel, TProperty> Parameter<TProperty>(Expression<Func<TModel, TProperty>> property);

        /// <summary>
        /// Configures an parameter property.
        /// </summary>
        /// <param name="propertyInfo"></param>
        /// <returns></returns>
        IParameterBuilder<TModel> Parameter(PropertyInfo propertyInfo);

        /// <summary>
        /// Configures an option property.
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="property"></param>
        /// <returns></returns>
        IOptionBuilder<TModel, TProperty> Option<TProperty>(Expression<Func<TModel, TProperty>> property);

        /// <summary>
        /// Configures an option property.
        /// </summary>
        /// <param name="propertyInfo"></param>
        /// <returns></returns>
        IOptionBuilder<TModel> Option(PropertyInfo propertyInfo);
    }
}