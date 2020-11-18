namespace Typin.Console.IO
{
    /// <summary>
    /// Abstraction for interacting with the standard input stream (stdin).
    /// </summary>
    public interface IStandardInput
    {
        /// <summary>
        /// Input stream (stdin) reader.
        /// </summary>
        StandardStreamReader Input { get; }
    }
}
