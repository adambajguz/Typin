namespace Typin.Models.Builders
{
    /// <summary>
    /// Represents a builder.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IBuilder<T>
    {
        /// <summary>
        /// Builds an instance of <typeparamref name="T"/>.
        /// </summary>
        /// <returns></returns>
        T Build();
    }
}
