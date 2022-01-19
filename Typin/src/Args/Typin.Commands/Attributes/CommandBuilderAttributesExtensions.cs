namespace Typin.Commands.Attributes
{
    using System;
    using System.ComponentModel;
    using System.Reflection;
    using Typin.Commands.Builders;
    using Typin.Schemas.Attributes;
    using Typin.Schemas.Builders;

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
        public static ICommandBuilder<TCommand> FromAttributes<TCommand>(this ICommandBuilder<TCommand> builder)
            where TCommand : class, ICommand
        {
            return builder.FromAttributesInner();
        }

        /// <summary>
        /// Configures the command from attributes.
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static ICommandBuilder FromAttributes(this ICommandBuilder builder)
        {
            return builder.FromAttributesInner();
        }

        /// <summary>
        /// Configures the command from attributes.
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        private static TSelf FromAttributesInner<TSelf>(this TSelf builder)
            where TSelf : class, IBaseCommandBuilder<TSelf>
        {
            Type commandType = builder.Model.Type;

            (builder as IManageAliases<TSelf>).FromAttributes();

            foreach (DescriptionAttribute descriptionAttribute in commandType.GetCustomAttributes<DescriptionAttribute>())
            {
                if (builder.Description is null)
                {
                    builder.UseDescription(descriptionAttribute.Description);
                }
                else if (descriptionAttribute.Description is not null)
                {
                    builder.Description += descriptionAttribute.Description;
                }
            }

            return builder;
        }
    }
}
