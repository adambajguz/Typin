namespace Typin.Models.Builders
{
    using System.Reflection;
    using Typin.Models.Schemas;
    using Typin.Schemas.Builders;

    /// <summary>
    /// Model builder.
    /// </summary>
    public interface IModelBuilder : IBuilder<IModelSchema>, IManageExtensions<IModelBuilder>
    {
        /// <summary>
        /// Configures an parameter property.
        /// </summary>
        /// <param name="propertyInfo"></param>
        /// <returns></returns>
        IParameterBuilder Parameter(PropertyInfo propertyInfo);

        /// <summary>
        /// Configures an option property.
        /// </summary>
        /// <param name="propertyInfo"></param>
        /// <returns></returns>
        IOptionBuilder Option(PropertyInfo propertyInfo);
    }
}