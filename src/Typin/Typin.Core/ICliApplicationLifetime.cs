namespace Typin
{
    using System;

    /// <summary>
    /// Provides methods to switch and get current CLI mode.
    /// </summary>
    public interface ICliApplicationLifetime
    {
        /// <summary>
        /// Current application state.
        /// </summary>
        CliLifetimes State { get; }

        /// <summary>
        /// Current mode or null if startup mode was not initialized.
        /// </summary>
        ICliMode? CurrentMode { get; }

        /// <summary>
        /// Current mode type or null if startup mode was not initialized.
        /// </summary>
        Type? CurrentModeType { get; }

        /// <summary>
        /// Requested to switch to or null if nothing needs to be changes.
        /// </summary>
        ICliMode? RequestedMode { get; }

        /// <summary>
        /// Requested mode type or null if startup mode was not initialized.
        /// </summary>
        Type? RequestedModeType { get; }

        /// <summary>
        /// Whether mode change was queued.
        /// </summary>
        bool IsModeRequested { get; }

        /// <summary>
        /// Queues a mode reset to startup mode when mode differs from current and pending.
        /// </summary>
        /// <returns>
        /// True if mode change was queued, otherwise false.
        /// </returns>
        bool ResetMode();

        /// <summary>
        /// Queues a mode change when mode differs from current and pending.
        /// </summary>
        /// <returns>
        /// True if mode change was queued, otherwise false.
        /// </returns>
        bool RequestMode<T>() where T : ICliMode;

        /// <summary>
        /// Queues a mode change when mode differs from current and pending.
        /// </summary>
        /// <returns>
        /// True if mode change was queued, otherwise false.
        /// </returns>
        bool RequestMode(Type cliMode);

        /// <summary>
        /// Queues a stop. Application will stop after the end of current command execution if there was no requested mode change.
        /// </summary>
        void RequestStop();
    }
}