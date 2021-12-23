namespace Typin.Internal.DynamicCommands
{
    using System;
    using System.Collections.Generic;
    using Typin.DynamicCommands;
    using Typin.Models.Builders;
    using Typin.Models.Schemas;
    using Typin.Schemas;

    /// <summary>
    /// Dynamic command builder.
    /// </summary>
    internal sealed class DynamicCommandBuilder : ModelBuilder, IDynamicCommandBuilder
    {
        private readonly Type _type;
        private readonly string _name;
        private string? _description;
        private string? _manual;
        private readonly List<Type> _supportedModes = new();
        private readonly List<Type> _excludedModes = new();

        private readonly List<IOptionSchema> _options = new();
        private readonly List<IParameterSchema> _parameters = new();

        /// <summary>
        /// Initializes a new instance of <see cref="DynamicCommandBuilder"/>.
        /// </summary>
        public DynamicCommandBuilder(RootSchema rootSchema, Type dynamicCommandType, string name) :
            base(dynamicCommandType)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException($"'{nameof(name)}' cannot be null or whitespace.", nameof(name));
            }

            _type = dynamicCommandType ?? throw new ArgumentNullException(nameof(dynamicCommandType));
            _name = name;

            //IModelSchema baseCommandSchema = rootSchema.TryFindDynamicCommandBase(_type) ??
            //    throw new NullReferenceException($"Dynamic command base not found for '{_type.FullName ?? _type.Name}'.");

            //_options = baseCommandSchema.Options.ToList();
            //_parameters = baseCommandSchema.Parameters.ToList();

            //_parameterNames = baseCommandSchema.Parameters.Select(x => x.Name)
            //    .Concat(baseCommandSchema.Options.Select(x => x.ShortName.ToString()).Where(x => x is not null))
            //    .Concat(baseCommandSchema.Options.Select(x => x.Name).Where(x => x is not null))
            //    .ToHashSet()!;
        }

        /// <inheritdoc/>
        public IDynamicCommandBuilder WithDescription(string? description)
        {
            _description = description;

            return this;
        }

        /// <inheritdoc/>
        public IDynamicCommandBuilder WithManual(string? manual)
        {
            _manual = manual;

            return this;
        }

        /// <inheritdoc/>
        public IDynamicCommandBuilder WithSupportedModes(params Type[] supportedModes)
        {
            _supportedModes.AddRange(supportedModes);

            return this;
        }

        /// <inheritdoc/>
        public IDynamicCommandBuilder WithExcludedModes(params Type[] excludedModes)
        {
            _excludedModes.AddRange(excludedModes);

            return this;
        }

        #region Option
        /// <inheritdoc/>
        public IDynamicCommandBuilder AddOption(Type optionType)
        {
            string name = string.Concat("Opt", _options.Count);

            return AddOption(optionType, name, ob => { });
        }

        /// <inheritdoc/>
        public IDynamicCommandBuilder AddOption<T>()
        {
            return AddOption<T>(ob => { });
        }

        /// <inheritdoc/>
        public IDynamicCommandBuilder AddOption(Type optionType, Action<IDynamicOptionBuilder> action)
        {
            string name = string.Concat("Opt", _options.Count);

            return AddOption(optionType, name, action);
        }

        /// <inheritdoc/>
        public IDynamicCommandBuilder AddOption<T>(Action<IDynamicOptionBuilder<T>> action)
        {
            string name = string.Concat("Opt", _options.Count);

            return AddOption<T>(name, action);
        }

        /// <inheritdoc/>
        public IDynamicCommandBuilder AddOption(Type optionType, string propertyName)
        {
            return AddOption(optionType, propertyName, ob => { });
        }

        /// <inheritdoc/>
        public IDynamicCommandBuilder AddOption<T>(string propertyName)
        {
            return AddOption<T>(propertyName, ob => { });
        }

        /// <inheritdoc/>
        public IDynamicCommandBuilder AddOption(Type optionType, string propertyName, Action<IDynamicOptionBuilder> action)
        {
            throw new NotImplementedException();

            return this;
        }

        /// <inheritdoc/>
        public IDynamicCommandBuilder AddOption<T>(string propertyName, Action<IDynamicOptionBuilder<T>> action)
        {
            throw new NotImplementedException();


            return this;
        }
        #endregion

        #region Parameter
        /// <inheritdoc/>
        public IDynamicCommandBuilder AddParameter(Type parameterType, int order)
        {
            return AddParameter(parameterType, order, pb => { });
        }

        /// <inheritdoc/>
        public IDynamicCommandBuilder AddParameter<T>(int order)
        {
            return AddParameter<T>(order, pb => { });
        }

        /// <inheritdoc/>
        public IDynamicCommandBuilder AddParameter(Type parameterType, int order, Action<IDynamicParameterBuilder> action)
        {
            string name = string.Concat("Param", _parameters.Count);

            return AddParameter(parameterType, name, order, action);
        }

        /// <inheritdoc/>
        public IDynamicCommandBuilder AddParameter<T>(int order, Action<IDynamicParameterBuilder<T>> action)
        {
            string name = string.Concat("Param", _parameters.Count);

            return AddParameter<T>(name, order, action);
        }

        /// <inheritdoc/>
        public IDynamicCommandBuilder AddParameter(Type parameterType, string propertyName, int order)
        {
            return AddParameter(parameterType, propertyName, order, ob => { });
        }

        /// <inheritdoc/>
        public IDynamicCommandBuilder AddParameter<T>(string propertyName, int order)
        {
            return AddParameter<T>(propertyName, order, ob => { });
        }

        /// <inheritdoc/>
        public IDynamicCommandBuilder AddParameter(Type parameterType, string propertyName, int order, Action<IDynamicParameterBuilder> action)
        {
            throw new NotImplementedException();

            return this;
        }

        /// <inheritdoc/>
        public IDynamicCommandBuilder AddParameter<T>(string propertyName, int order, Action<IDynamicParameterBuilder<T>> action)
        {
            throw new NotImplementedException();

            return this;
        }
        #endregion

        public ICommandSchema Build()
        {
            throw new NotImplementedException();
        }
    }
}
