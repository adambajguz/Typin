namespace Typin.Directives.Attributes
{
    using System;
    using System.ComponentModel;
    using System.Reflection;
    using Typin.Directives.Builders;
    using Typin.Schemas.Attributes;

    /// <summary>
    /// Directive builder attributes configuration extensions.
    /// </summary>
    public static class DirectiveBuilderAttributesExtensions
    {
        /// <summary>
        /// Configures the directive from attributes.
        /// </summary>
        /// <typeparam name="TDirective"></typeparam>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IDirectiveBuilder<TDirective> FromAttributes<TDirective>(this IDirectiveBuilder<TDirective> builder)
            where TDirective : class, IDirective
        {
            return builder.FromAttributesInner();
        }

        /// <summary>
        /// Configures the directive from attributes.
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IDirectiveBuilder FromAttributes(this IDirectiveBuilder builder)
        {
            return builder.FromAttributesInner();
        }

        /// <summary>
        /// Configures the directive from attributes.
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        private static TSelf FromAttributesInner<TSelf>(this TSelf builder)
            where TSelf : class, IBaseDirectiveBuilder<TSelf>
        {
            Type directiveType = builder.Model.Type;
            builder.FromAttributes();

            foreach (DescriptionAttribute descriptionAttribute in directiveType.GetCustomAttributes<DescriptionAttribute>())
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
