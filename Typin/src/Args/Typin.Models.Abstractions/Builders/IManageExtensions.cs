namespace Typin.Models.Builders
{
    using System;
    using Typin.Schemas.Collections;

    /// <summary>
    /// Extensions management.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IManageExtensions<T>
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
        T ManageExtensions(Action<IExtensionsCollection> action);
    }
}
