namespace Typin.Schemas.Builders
{
    using System;
    using Typin.Schemas.Collections;

    /// <summary>
    /// Aliases management.
    /// </summary>
    /// <typeparam name="TSelf"></typeparam>
    public interface IManageAliases<TSelf> : ISelf<TSelf>, IModelTypeAccessor
        where TSelf : class, IManageAliases<TSelf>
    {
        /// <summary>
        /// A collection of Aliases.
        /// </summary>
        IAliasCollection Aliases { get; }

        /// <summary>
        /// A fluent builder compatible method for managing Aliases (alternative to <see cref="Aliases"/>.
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        TSelf ManageAliases(Action<IAliasCollection> action);
    }
}
