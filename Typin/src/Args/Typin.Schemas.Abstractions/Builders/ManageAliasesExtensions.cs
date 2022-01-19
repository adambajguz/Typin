namespace Typin.Schemas.Builders
{
    /// <summary>
    /// <see cref="IManageAliases{TSelf}"/>
    /// </summary>
    public static class ManageAliasesExtensions
    {
        /// <summary>
        /// Adds a default alias.
        /// </summary>
        /// <typeparam name="TParentBuilder"></typeparam>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static TParentBuilder AddDefaultAlias<TParentBuilder>(this IManageAliases<TParentBuilder> builder)
            where TParentBuilder : class, IManageAliases<TParentBuilder>
        {
            return builder.AddAlias(string.Empty);
        }

        /// <summary>
        /// Removes a default alias.
        /// </summary>
        /// <typeparam name="TParentBuilder"></typeparam>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static TParentBuilder RemoveDefaultAlias<TParentBuilder>(this IManageAliases<TParentBuilder> builder)
            where TParentBuilder : class, IManageAliases<TParentBuilder>
        {
            return builder.RemoveAlias(string.Empty);
        }

        /// <summary>
        /// Adds an alias if <paramref name="alias"/> is not null.
        /// </summary>
        /// <typeparam name="TParentBuilder"></typeparam>
        /// <param name="builder"></param>
        /// <param name="alias"></param>
        /// <returns></returns>
        public static TParentBuilder AddAlias<TParentBuilder>(this IManageAliases<TParentBuilder> builder, string? alias)
            where TParentBuilder : class, IManageAliases<TParentBuilder>
        {
            if (alias is not null)
            {
                builder.Aliases.Add(alias);
            }

            return builder.Self;
        }

        /// <summary>
        /// Adds an alias if <paramref name="alias"/> is not null.
        /// </summary>
        /// <typeparam name="TParentBuilder"></typeparam>
        /// <param name="builder"></param>
        /// <param name="alias"></param>
        /// <returns></returns>
        public static TParentBuilder RemoveAlias<TParentBuilder>(this IManageAliases<TParentBuilder> builder, string? alias)
            where TParentBuilder : class, IManageAliases<TParentBuilder>
        {
            if (alias is not null)
            {
                builder.Aliases.Remove(alias);
            }

            return builder.Self;
        }

        /// <summary>
        /// Adds an alias if <paramref name="alias"/> is not null.
        /// </summary>
        /// <typeparam name="TParentBuilder"></typeparam>
        /// <param name="builder"></param>
        /// <param name="alias"></param>
        /// <returns></returns>
        public static TParentBuilder ClearAliases<TParentBuilder>(this IManageAliases<TParentBuilder> builder, string? alias)
            where TParentBuilder : class, IManageAliases<TParentBuilder>
        {
            if (alias is not null)
            {
                builder.Aliases.Remove(alias);
            }

            return builder.Self;
        }
    }
}
