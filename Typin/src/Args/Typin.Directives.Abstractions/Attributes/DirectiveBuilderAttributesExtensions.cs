namespace Typin.Directives.Attributes
{
    using System;
    using System.Reflection;
    using Typin.Directives.Builders;

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
        public static IDirectiveBuilder<TDirective> FromAttribute<TDirective>(this IDirectiveBuilder<TDirective> builder)
            where TDirective : class, IDirective
        {
            Type directiveType = builder.Model.Type;
            DirectiveAttribute? attribute = directiveType.GetCustomAttribute<DirectiveAttribute>();

            if (attribute is not null)
            {
                builder.Name(attribute.Name)
                       .Alias(attribute.Alias)
                       .Description(attribute.Description);
            }

            return builder;
        }

        /// <summary>
        /// Configures the directive from attributes.
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IDirectiveBuilder FromAttribute(this IDirectiveBuilder builder)
        {
            Type directiveType = builder.Model.Type;
            DirectiveAttribute? attribute = directiveType.GetCustomAttribute<DirectiveAttribute>();

            if (attribute is not null)
            {
                builder.Name(attribute.Name)
                       .Alias(attribute.Alias)
                       .Description(attribute.Description);
            }

            return builder;
        }
    }
}
