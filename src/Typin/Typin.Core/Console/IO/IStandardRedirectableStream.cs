namespace Typin.Console
{
    /// <summary>
    /// Abstraction for standard stream redirtection state.
    /// </summary>
    public interface IStandardRedirectableStream
    {
        /// <summary>
        /// Whether the stream is redirected.
        /// </summary>
        bool IsRedirected { get; }
    }
}