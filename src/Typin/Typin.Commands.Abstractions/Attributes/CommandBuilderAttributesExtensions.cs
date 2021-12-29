namespace Typin.Commands.Attributes
{
    using System;
    using System.Reflection;
    using Typin.Commands.Builders;

    /// <summary>
    /// Command builder attributes configuration extensions.
    /// </summary>
    public static class CommandBuilderAttributesExtensions
    {
        /// <summary>
        /// Configures the command from attributes.
        /// </summary>
        /// <typeparam name="TCommand"></typeparam>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static ICommandBuilder<TCommand> FromAttribute<TCommand>(this ICommandBuilder<TCommand> builder)
            where TCommand : class, ICommand
        {
            Type commandType = builder.Model.Type;
            CommandAttribute? attribute = commandType.GetCustomAttribute<CommandAttribute>();

            if (attribute is not null)
            {
                builder.Name(attribute.Name)
                       .Description(attribute.Description); //TODO: handle manuals.
            }

            return builder;
        }

        /// <summary>
        /// Configures the command from attributes.
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static ICommandBuilder FromAttribute(this ICommandBuilder builder)
        {
            Type commandType = builder.Model.Type;
            CommandAttribute? attribute = commandType.GetCustomAttribute<CommandAttribute>();

            if (attribute is not null)
            {
                builder.Name(attribute.Name)
                       .Description(attribute.Description); //TODO: handle manuals.
            }

            return builder;
        }
    }
}
