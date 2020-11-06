namespace Typin
{
    using System;

    /// <summary>
    /// CLI mode switcher.
    /// </summary>
    public interface ICliModeSwitcher
    {
        /// <summary>
        /// Current CLI mode.
        /// </summary>
        ICliMode Current { get; }

        /// <summary>
        /// Whether mode change was queued.
        /// </summary>
        bool IsPending { get; }

        /// <summary>
        /// Pending CLI mode to apply.
        /// </summary>
        ICliMode? Pending { get; }

        /// <summary>
        /// Queues mode change.
        /// </summary>
        bool SwitchMode<ICliMode>();

        /// <summary>
        /// Queues mode change.
        /// </summary>
        bool SwitchMode(Type cliMode);
    }
}