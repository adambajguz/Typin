namespace Typin.Models.Schemas
{
    /// <summary>
    /// Parameter schema.
    /// </summary>
    public interface IParameterSchema : IArgumentSchema
    {
        /// <summary>
        /// Parameter order.
        /// </summary>
        int Order { get; }

        /// <summary>
        /// Parameter name.
        /// </summary>
        new string Name { get; }
    }
}