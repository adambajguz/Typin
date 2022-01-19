namespace Typin.Schemas.Attributes
{
    using System.Reflection;
    using Typin.Schemas.Builders;

    /// <summary>
    /// Alias builder attributes configuration extensions.
    /// </summary>
    public static class ManageAliasesAttributesExtensions
    {
        /// <summary>
        /// Configures an aliases from attributes.
        /// </summary>
        /// <typeparam name="TParentBuilder"></typeparam>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static TParentBuilder FromAttributes<TParentBuilder>(this IManageAliases<TParentBuilder> builder)
            where TParentBuilder : class, IManageAliases<TParentBuilder>
        {
            foreach (AliasAttribute aliasAttribute in builder.ModelType.GetCustomAttributes<AliasAttribute>())
            {
                if (aliasAttribute.Value is string value)
                {
                    builder.Aliases.Add(value);
                }
            }

            return builder.Self;
        }
    }
}
