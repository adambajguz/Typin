namespace Typin.Console.IO
{
    /// <summary>
    /// Abstraction for standard stream redirtection state.
    /// </summary>
    public interface IStandardRedirectableConsoleStream
    {
        /// <summary>
        /// Console instance that is using this stream.
        /// This property allows writing stream-targeted extensions.
        /// </summary>
        public IConsole BoundedConsole { get; }

        /// <summary>
        /// Whether the stream is redirected.
        /// </summary>
        bool IsRedirected { get; }
    }
}