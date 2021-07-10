namespace Typin.DynamicCommands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Typin.Schemas;

    /// <summary>
    /// Dynamic command builder.
    /// </summary>
    internal sealed class DynamicCommandBuilder : IDynamicCommandBuilder
    {
        private readonly BaseCommandSchema _baseCommandSchema;

        private readonly Type _type;
        private readonly string _name;
        private string? _description;
        private string? _manual;
        private readonly List<Type> _supportedModes = new();
        private readonly List<Type> _excludedModes = new();

        private readonly HashSet<string> _parameterNames;

        private readonly List<OptionSchema> _options;
        private readonly List<ParameterSchema> _parameters;

        /// <summary>
        /// Initializes a new instance of <see cref="DynamicCommandBuilder"/>.
        /// </summary>
        public DynamicCommandBuilder(RootSchema rootSchema, Type dynamicCommandType, string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException($"'{nameof(name)}' cannot be null or whitespace.", nameof(name));
            }

            _type = dynamicCommandType ?? throw new ArgumentNullException(nameof(dynamicCommandType));
            _name = name;

            BaseCommandSchema baseCommandSchema = rootSchema.TryFindDynamicCommandBase(_type) ??
                throw new NullReferenceException($"Dynamic command base not found for '{_type.FullName ?? _type.Name}'.");

            _baseCommandSchema = baseCommandSchema;

            _options = baseCommandSchema.Options.ToList();
            _parameters = baseCommandSchema.Parameters.ToList();

            _parameterNames = baseCommandSchema.Parameters.Select(x => x.Name)
                .Concat(baseCommandSchema.Options.Select(x => x.ShortName.ToString()).Where(x => x is not null))
                .Concat(baseCommandSchema.Options.Select(x => x.Name).Where(x => x is not null))
                .ToHashSet()!;
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
            string name = string.Concat("opt", _options.Count);

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
            string name = string.Concat("opt", _options.Count);

            return AddOption(optionType, name, action);
        }

        /// <inheritdoc/>
        public IDynamicCommandBuilder AddOption<T>(Action<IDynamicOptionBuilder<T>> action)
        {
            string name = string.Concat("opt", _options.Count);

            return AddOption<T>(name, action);
        }

        /// <inheritdoc/>
        public IDynamicCommandBuilder AddOption(Type optionType, string name)
        {
            return AddOption(optionType, name, ob => { });
        }

        /// <inheritdoc/>
        public IDynamicCommandBuilder AddOption<T>(string name)
        {
            return AddOption<T>(name, ob => { });
        }

        /// <inheritdoc/>
        public IDynamicCommandBuilder AddOption(Type optionType, string name, Action<IDynamicOptionBuilder> action)
        {
            DynamicOptionBuilder builder = new(name, optionType);
            action(builder);

            OptionSchema schema = builder.Build();
            _options.Add(schema);

            if (schema.Name is not null)
            {
                _parameterNames.Add(schema.Name);
            }

            if (schema.ShortName is not null)
            {
                _parameterNames.Add(schema.ShortName.ToString()!);
            }

            return this;
        }

        /// <inheritdoc/>
        public IDynamicCommandBuilder AddOption<T>(string name, Action<IDynamicOptionBuilder<T>> action)
        {
            DynamicOptionBuilder<T> builder = new(name);
            action(builder);

            OptionSchema schema = builder.Build();
            _options.Add(schema);

            if (schema.Name is not null)
            {
                _parameterNames.Add(schema.Name);
            }

            if (schema.ShortName is not null)
            {
                _parameterNames.Add(schema.ShortName.ToString()!);
            }

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
            string name = string.Concat("param", _parameters.Count);

            return AddParameter(parameterType, name, order, action);
        }

        /// <inheritdoc/>
        public IDynamicCommandBuilder AddParameter<T>(int order, Action<IDynamicParameterBuilder<T>> action)
        {
            string name = string.Concat("param", _parameters.Count);

            return AddParameter<T>(name, order, action);
        }

        /// <inheritdoc/>
        public IDynamicCommandBuilder AddParameter(Type parameterType, string name, int order)
        {
            return AddParameter(parameterType, name, order, ob => { });
        }

        /// <inheritdoc/>
        public IDynamicCommandBuilder AddParameter<T>(string name, int order)
        {
            return AddParameter<T>(name, order, ob => { });
        }

        /// <inheritdoc/>
        public IDynamicCommandBuilder AddParameter(Type parameterType, string name, int order, Action<IDynamicParameterBuilder> action)
        {
            DynamicParameterBuilder builder = new(parameterType, name, order);
            action(builder);

            ParameterSchema schema = builder.Build();
            _parameters.Add(schema);

            _parameterNames.Add(schema.Name);

            return this;
        }

        /// <inheritdoc/>
        public IDynamicCommandBuilder AddParameter<T>(string name, int order, Action<IDynamicParameterBuilder<T>> action)
        {
            DynamicParameterBuilder<T> builder = new(name, order);
            action(builder);

            ParameterSchema schema = builder.Build();
            _parameters.Add(schema);

            _parameterNames.Add(schema.Name);

            return this;
        }
        #endregion

        public CommandSchema Build()
        {
            return new CommandSchema(_type,
                                     true,
                                     _name,
                                     _description,
                                     _manual,
                                     _supportedModes.Count == 0 ? null : _supportedModes,
                                     _excludedModes.Count == 0 ? null : _excludedModes,
                                     _parameters,
                                     _options);
        }
    }
}
