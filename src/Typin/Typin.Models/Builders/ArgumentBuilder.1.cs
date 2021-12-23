namespace Typin.Models.Builders
{
    using System.Reflection;

    /// <summary>
    /// Argument builder.
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    internal abstract class ArgumentBuilder<TModel>
        where TModel : class, IModel
    {
        /// <summary>
        /// Property info.
        /// </summary>
        protected PropertyInfo Property { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="ArgumentBuilder{TModel}"/>.
        /// </summary>
        /// <param name="propertyInfo"></param>
        public ArgumentBuilder(PropertyInfo propertyInfo)
        {
            Property = propertyInfo;
        }
    }
}
