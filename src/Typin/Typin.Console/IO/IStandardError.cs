namespace Typin.Console.IO
{
    /// <summary>
    /// Abstraction for interacting with the standard error stream (stderr).
    /// </summary>
    public interface IStandardError
    {
        /// <summary>
        /// Error stream (stderr) writer.
        /// </summary>
        StandardStreamWriter Error { get; }
    }
}
