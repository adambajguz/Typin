namespace Typin.Schemas.Builders
{
    using System;
    using Typin.Schemas.Collections;

    /// <summary>
    /// Extensions management.
    /// </summary>
    /// <typeparam name="TSelf"></typeparam>
    public interface IManageExtensions<TSelf> : ISelf<TSelf>, IModelTypeAccessor
        where TSelf : class, IManageExtensions<TSelf>
    {
        /// <summary>
        /// A collection of extensions.
        /// </summary>
        IExtensionsCollection Extensions { get; }

        /// <summary>
        /// A fluent builder compatible method for managing extensions (alternative to <see cref="Extensions"/>.
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        TSelf ManageExtensions(Action<IExtensionsCollection> action);
    }
}
