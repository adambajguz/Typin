namespace Typin.DynamicCommands
{
    using System;
    using System.Collections.Generic;
    using Typin.Schemas;

    /// <summary>
    /// Dynamic command builder.
    /// </summary>
    internal sealed class DynamicCommandBuilder : IDynamicCommandBuilder
    {
        private string? _name;
        private string? _description;
        private string? _manual;
        private List<Type> _supportedModes = new();
        private List<Type> _excludedModes = new();

        /// <summary>
        /// Initializes a new instance of <see cref="DynamicCommandBuilder"/>.
        /// </summary>
        public DynamicCommandBuilder()
        {

        }

        /// <inheritdoc/>
        public IDynamicCommandBuilder WithName(string? name)
        {
            _name = name;

            return this;
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

        /// <inheritdoc/>
        public IDynamicCommandBuilder AddOption(Type optionType, Action<IDynamicOptionBuilder> action)
        {
            DynamicOptionBuilder builder = new(optionType);
            action(builder);

            return this;
        }

        /// <inheritdoc/>
        public IDynamicCommandBuilder AddOption<T>(Action<IDynamicOptionBuilder<T>> action)
        {
            DynamicOptionBuilder<T> builder = new();
            action(builder);

            return this;
        }

        /// <inheritdoc/>
        public IDynamicCommandBuilder AddParameter(Type parameterType, int order, Action<IDynamicParameterBuilder> action)
        {
            DynamicParameterBuilder builder = new(parameterType, order);
            action(builder);

            return this;
        }

        /// <inheritdoc/>
        public IDynamicCommandBuilder AddParameter<T>(int order, Action<IDynamicParameterBuilder<T>> action)
        {
            DynamicParameterBuilder<T> builder = new(order);
            action(builder);

            return this;
        }

        public CommandSchema Build()
        {
            throw new NotImplementedException();
        }
    }
}
