namespace Typin.Console.IO
{
    /// <summary>
    /// Abstraction for interacting with the standard error stream.
    /// </summary>
    public interface IStandardError
    {
        /// <summary>
        /// Error stream (stderr).
        /// </summary>
        StandardStreamWriter Error { get; }
    }
}
