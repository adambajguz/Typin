namespace Typin.Models.Builders
{
    using System;
    using System.Linq.Expressions;
    using System.Reflection;
    using Typin.Models.Extensions;

    /// <summary>
    /// Argument builder.
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    /// <typeparam name="TProperty"></typeparam>
    internal abstract class ArgumentBuilder<TModel, TProperty>
        where TModel : class, IModel
    {
        /// <summary>
        /// Property info.
        /// </summary>
        protected PropertyInfo Property { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="OptionBuilder{TModel, TProperty}"/>.
        /// </summary>
        /// <param name="propertyAccessor"></param>
        public ArgumentBuilder(Expression<Func<TModel, TProperty>> propertyAccessor)
        {
            Property = propertyAccessor.GetPropertyInfo();
        }
    }
}
