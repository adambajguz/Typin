namespace Typin.Console
{
    /// <summary>
    /// Abstraction for interacting with the standard input stream.
    /// </summary>
    public interface IStandardInput
    {
        /// <summary>
        /// Input stream (stdin).
        /// </summary>
        StandardStreamReader Input { get; }
    }
}
