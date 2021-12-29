namespace Typin.Models.Builders
{
    /// <summary>
    /// Represents a resolver.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IResolver<T>
    {
        /// <summary>
        /// Resolves an instance of <typeparamref name="T"/>.
        /// </summary>
        /// <returns></returns>
        T Resolve();
    }
}
