namespace Typin.Models.Builders
{
    using System;
    using System.Linq.Expressions;
    using System.Reflection;
    using Typin.Schemas.Builders;
    using Typin.Schemas.Collections;

    /// <summary>
    /// Model builder.
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    public class ModelBuilder<TModel> : ModelBuilder, IModelBuilder<TModel>
        where TModel : class, IModel
    {
        IModelBuilder<TModel> ISelf<IModelBuilder<TModel>>.Self => this;

        /// <summary>
        /// Initializes a new instance of <see cref="ModelBuilder{TModel}"/>.
        /// </summary>
        public ModelBuilder() :
            base(typeof(TModel))
        {

        }

        IModelBuilder<TModel> IManageExtensions<IModelBuilder<TModel>>.ManageExtensions(Action<IExtensionsCollection> action)
        {
            action(Extensions);

            return this;
        }

        /// <inheritdoc/>
        public IParameterBuilder<TModel, TProperty> Parameter<TProperty>(Expression<Func<TModel, TProperty>> property)
        {
            ParameterBuilder<TModel, TProperty> builder = new(Parameters.Count, property);
            AddParameterBuilder(builder);

            return builder;
        }

        /// <inheritdoc/>
        public IOptionBuilder<TModel, TProperty> Option<TProperty>(Expression<Func<TModel, TProperty>> property)
        {
            OptionBuilder<TModel, TProperty> builder = new(property);
            AddOptionBuilder(builder);

            return builder;
        }

        IParameterBuilder<TModel> IModelBuilder<TModel>.Parameter(PropertyInfo propertyInfo)
        {
            ParameterBuilder<TModel> builder = new(Parameters.Count, propertyInfo);
            AddParameterBuilder(builder);

            return builder;
        }

        IOptionBuilder<TModel> IModelBuilder<TModel>.Option(PropertyInfo propertyInfo)
        {
            OptionBuilder<TModel> builder = new(propertyInfo);
            AddOptionBuilder(builder);

            return builder;
        }
    }
}
