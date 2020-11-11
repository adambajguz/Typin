namespace Typin
{
    /// <summary>
    /// CLI application lifetimes.
    /// </summary>
    public enum CliLifetimes
    {
        /// <summary>
        /// Application is starting.
        /// </summary>
        Starting,

        /// <summary>
        /// Application is running.
        /// </summary>
        Running,

        /// <summary>
        /// A stop was requested.
        /// </summary>
        StopRequested,

        /// <summary>
        /// App is stopped.
        /// </summary>
        Stopped
    }
}