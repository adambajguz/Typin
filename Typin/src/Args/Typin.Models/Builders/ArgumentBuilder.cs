namespace Typin.Models.Builders
{
    using System;
    using System.Reflection;
    using Typin.Schemas.Collections;

    /// <summary>
    /// Argument builder.
    /// </summary>
    internal abstract class ArgumentBuilder
    {
        private bool _built;

        /// <summary>
        /// Model type.
        /// </summary>
        public Type ModelType { get; }

        /// <summary>
        /// Property info.
        /// </summary>
        protected PropertyInfo Property { get; }

        /// <summary>
        /// A collection of extensions.
        /// </summary>
        public IExtensionsCollection Extensions { get; } = new ExtensionsCollection();

        /// <summary>
        /// Initializes a new instance of <see cref="ArgumentBuilder"/>.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="propertyInfo"></param>
        public ArgumentBuilder(Type model, PropertyInfo propertyInfo)
        {
            ModelType = model;
            Property = propertyInfo;
        }

        /// <summary>
        /// Throws <see cref="InvalidOperationException"/> when built more then once.
        /// It is recommended to call this method just before return to allow rebuild on exception.
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        protected void EnsureBuiltOnce()
        {
            if (_built)
            {
                throw new InvalidOperationException($"Argument '{ModelType}.{Property}' was already built.");
            }

            _built = true;
        }
    }
}
