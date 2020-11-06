namespace Typin
{
    /// <summary>
    /// CLI mode switcher.
    /// </summary>
    public interface ICliModeSwitcher
    {
        /// <summary>
        /// Current CLI mode.
        /// </summary>
        CliModes Current { get; }

        /// <summary>
        /// Whether mode change was queued.
        /// </summary>
        bool IsPending { get; }

        /// <summary>
        /// Pending CLI mode to apply.
        /// </summary>
        CliModes? Pending { get; }

        /// <summary>
        /// Queues mode change.
        /// </summary>
        void QueueSwitching(CliModes mode);
    }
}