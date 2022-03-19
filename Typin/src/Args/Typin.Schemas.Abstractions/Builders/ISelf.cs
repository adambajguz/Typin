namespace Typin.Schemas.Builders
{
    /// <summary>
    /// Represents a class with strongly typed self reference.
    /// </summary>
    /// <typeparam name="TSelf"></typeparam>
    public interface ISelf<TSelf>
        where TSelf : class, ISelf<TSelf>
    {
        /// <summary>
        /// Self.
        /// </summary>
        TSelf Self { get; }
    }
}
