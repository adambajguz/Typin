﻿namespace Typin
{
    using System;

    /// <summary>
    /// Provides methods to switch and get current CLI mode.
    /// </summary>
    public interface ICliModeSwitcher
    {
        /// <summary>
        /// Current mode or null if startup mode was not initialized.
        /// </summary>
        ICliMode? Current { get; }

        /// <summary>
        /// Current mode type or null if startup mode was not initialized.
        /// </summary>
        Type? CurrentType { get; }

        /// <summary>
        /// Mode to switch to or null if nothing needs to be changes.
        /// </summary>
        ICliMode? Pending { get; }

        /// <summary>
        /// Pending mode type or null if startup mode was not initialized.
        /// </summary>
        Type? PendingType { get; }

        /// <summary>
        /// Whether mode change was queued.
        /// </summary>
        bool IsPending { get; }

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
    }
}