namespace Typin.Internal.DynamicCommands
{
    using System;
    using Typin.DynamicCommands;
    using Typin.Schemas;

    /// <summary>
    /// Dynamic command builder factory.
    /// </summary>
    internal class DynamicCommandBuilderFactory : IDynamicCommandBuilderFactory
    {
        private readonly IRootSchemaAccessor _rootSchemaAccessor;

        /// <summary>
        /// Initializes a new instance of <see cref="DynamicCommandBuilderFactory"/>.
        /// </summary>
        public DynamicCommandBuilderFactory(IRootSchemaAccessor rootSchemaAccessor)
        {
            _rootSchemaAccessor = rootSchemaAccessor;
        }

        /// <inheritdoc/>
        public IDynamicCommandBuilder Create(Type dynamicCommandType, string commandName)
        {
            if (!KnownTypesHelpers.IsCommandTemplateType(dynamicCommandType))
            {
                throw new ArgumentException(
                    $"Type '{dynamicCommandType.FullName ?? dynamicCommandType.Name}' does not represent a dynamic command, i.e., does not implement '{nameof(ICommandTemplate)}'.",
                    nameof(dynamicCommandType));
            }

            return new DynamicCommandBuilder(_rootSchemaAccessor.RootSchema, dynamicCommandType, commandName);
        }

        /// <inheritdoc/>
        public IDynamicCommandBuilder Create<T>(string commandName)
            where T : class, ICommandTemplate
        {
            return new DynamicCommandBuilder(_rootSchemaAccessor.RootSchema, typeof(T), commandName);
        }
    }
}
