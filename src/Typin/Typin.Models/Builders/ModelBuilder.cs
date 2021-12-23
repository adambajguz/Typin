namespace Typin.Models.Builders
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Reflection;
    using Typin.Models.Binding;
    using Typin.Models.Schemas;

    /// <summary>
    /// Model builder.
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    internal sealed class ModelBuilder<TModel> : IModelBuilder<TModel>
        where TModel : class, IModel
    {
        private readonly List<IParameterSchema> _parameters = new();
        private readonly List<IOptionSchema> _options = new();

        private IBuilder<IParameterSchema>? _currentParameterBuilder;
        private IBuilder<IOptionSchema>? _currentOptionBuilder;

        /// <summary>
        /// Initializes a new instance of <see cref="ModelBuilder{TModel}"/>.
        /// </summary>
        public ModelBuilder()
        {

        }

        /// <inheritdoc/>
        public IParameterBuilder<TModel, TProperty> Parameter<TProperty>(Expression<Func<TModel, TProperty>> property)
        {
            if (_currentParameterBuilder is not null)
            {
                _parameters.Add(_currentParameterBuilder.Build());
            }

            ParameterBuilder<TModel, TProperty> builder = new(_parameters.Count, property);
            _currentParameterBuilder = builder;

            return builder;
        }

        /// <inheritdoc/>
        public IParameterBuilder<TModel> Parameter(PropertyInfo propertyInfo)
        {
            if (_currentParameterBuilder is not null)
            {
                _parameters.Add(_currentParameterBuilder.Build());
            }

            ParameterBuilder<TModel> builder = new(_parameters.Count, propertyInfo);
            _currentParameterBuilder = builder;

            return builder;
        }

        /// <inheritdoc/>
        public IOptionBuilder<TModel, TProperty> Option<TProperty>(Expression<Func<TModel, TProperty>> property)
        {
            if (_currentOptionBuilder is not null)
            {
                _options.Add(_currentOptionBuilder.Build());
            }

            OptionBuilder<TModel, TProperty> builder = new(property);
            _currentOptionBuilder = builder;

            return builder;
        }

        /// <inheritdoc/>
        public IOptionBuilder<TModel> Option(PropertyInfo propertyInfo)
        {
            if (_currentOptionBuilder is not null)
            {
                _options.Add(_currentOptionBuilder.Build());
            }

            OptionBuilder<TModel> builder = new(propertyInfo);
            _currentOptionBuilder = builder;

            return builder;
        }

        /// <inheritdoc/>
        public IModelSchema Build()
        {
            if (_currentParameterBuilder is not null)
            {
                _parameters.Add(_currentParameterBuilder.Build());
            }

            if (_currentOptionBuilder is not null)
            {
                _options.Add(_currentOptionBuilder.Build());
            }

            return new ModelSchema(typeof(TModel),
                                   _parameters,
                                   _options);
        }
    }
}
