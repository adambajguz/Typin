namespace Typin.Models.Builders
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using Typin.Models.Collections;
    using Typin.Models.Schemas;

    /// <summary>
    /// Model builder.
    /// </summary>
    public class ModelBuilder : IModelBuilder
    {
        private bool _built;

        /// <summary>
        /// A collection of parameters.
        /// </summary>
        protected List<IParameterSchema> Parameters { get; } = new();

        /// <summary>
        /// A collection of options.
        /// </summary>
        protected List<IOptionSchema> Options { get; } = new();

        /// <summary>
        /// A collection of extensions.
        /// </summary>
        public IExtensionsCollection Extensions { get; } = new ExtensionsCollection();

        private IBuilder<IParameterSchema>? _currentParameterBuilder;
        private IBuilder<IOptionSchema>? _currentOptionBuilder;

        /// <summary>
        /// Model type.
        /// </summary>
        protected Type Model { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="ModelBuilder{TModel}"/>.
        /// </summary>
        public ModelBuilder(Type model)
        {
            Model = model;
        }

        IModelBuilder IManageExtensions<IModelBuilder>.ManageExtensions(Action<IExtensionsCollection> action)
        {
            action(Extensions);

            return this;
        }

        /// <inheritdoc/>
        public IParameterBuilder Parameter(PropertyInfo propertyInfo)
        {
            ParameterBuilder builder = new(Model, Parameters.Count, propertyInfo);
            AddParameterBuilder(builder);

            return builder;
        }

        /// <summary>
        /// Build last parameter (if exists), than add a new parameter builder.
        /// </summary>
        /// <param name="builder"></param>
        protected void AddParameterBuilder(IBuilder<IParameterSchema> builder)
        {
            if (_currentParameterBuilder is not null)
            {
                Parameters.Add(_currentParameterBuilder.Build());
            }

            _currentParameterBuilder = builder;
        }

        /// <inheritdoc/>
        public IOptionBuilder Option(PropertyInfo propertyInfo)
        {
            OptionBuilder builder = new(Model, propertyInfo);
            AddOptionBuilder(builder);

            return builder;
        }

        /// <summary>
        /// Build last option (if exists), than add a new option builder.
        /// </summary>
        /// <param name="builder"></param>
        protected void AddOptionBuilder(IBuilder<IOptionSchema> builder)
        {
            if (_currentOptionBuilder is not null)
            {
                Options.Add(_currentOptionBuilder.Build());
            }

            _currentOptionBuilder = builder;
        }

        /// <inheritdoc/>
        public IModelSchema Build()
        {
            if (_currentParameterBuilder is not null)
            {
                Parameters.Add(_currentParameterBuilder.Build());
            }

            if (_currentOptionBuilder is not null)
            {
                Options.Add(_currentOptionBuilder.Build());
            }

            ModelSchema schema = new(Model,
                                     Parameters,
                                     Options,
                                     Extensions);

            EnsureBuiltOnce(); // Call before return to allow rebuild on exception.
            return schema;
        }

        /// <summary>
        /// Throws <see cref="InvalidOperationException"/> when built more then once.
        /// It is recommended to call this method just before return to allow rebuild on exception.
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        protected void EnsureBuiltOnce()
        {
            if (_built)
            {
                throw new InvalidOperationException($"Model was '{Model}' already built.");
            }

            _built = true;
        }
    }
}
