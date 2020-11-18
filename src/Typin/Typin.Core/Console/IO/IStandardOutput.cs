namespace Typin.Console.IO
{
    /// <summary>
    /// Abstraction for interacting with the standard output stream (stdout).
    /// </summary>
    public interface IStandardOutput
    {
        /// <summary>
        /// Output stream (stdout) writer.
        /// </summary>
        StandardStreamWriter Output { get; }
    }
}
