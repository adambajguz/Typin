namespace Typin.Console
{
    /// <summary>
    /// Abstraction for interacting with the standard output stream.
    /// </summary>
    public interface IStandardOutput
    {
        /// <summary>
        /// Output stream (stdout).
        /// </summary>
        StandardStreamWriter Output { get; }
    }
}
