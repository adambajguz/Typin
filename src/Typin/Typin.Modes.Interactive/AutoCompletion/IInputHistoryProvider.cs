namespace Typin.Modes.Interactive.AutoCompletion
{
    using System.Collections.Generic;

    /// <summary>
    /// Input command history provider for interactive mode.
    /// </summary>
    public interface IInputHistoryProvider
    {
        /// <summary>
        /// Whether history is enabled.
        /// </summary>
        bool IsEnabled { get; set; }

        /// <summary>
        /// Tries to adds entry to the history. If entry wasn't added (because is whitespace) returns false.
        /// </summary>
        bool TryAddEntry(string entry);

        /// <summary>
        /// Adds multiple entries to the history.
        /// </summary>
        void AddEntries(params string[] entries);

        /// <summary>
        /// Removes last entry from the history.
        /// </summary>
        void RemoveLastEntry();

        /// <summary>
        /// Returns history. If <see cref="IsEnabled"/> is set to false, returns empty collection even if history is non-empty.
        /// </summary>
        IReadOnlyCollection<string> GetEntries();

        /// <summary>
        /// Clears the history.
        /// </summary>
        void Clear();
    }
}