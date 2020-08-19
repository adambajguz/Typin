namespace Typin.AutoCompletion
{
    using System.Collections.Generic;

    /// <summary>
    /// Provides command input history in interactive mode.
    /// </summary>
    public class InputHistoryProvider
    {
        private readonly LinkedList<string> _history = new LinkedList<string>();
        private LinkedListNode<string>? _selection;

        /// <summary>
        /// Whether history is enabled.
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// Initializes an instance of <see cref="InputHistoryProvider"/>.
        /// </summary>
        internal InputHistoryProvider()
        {

        }

        /// <summary>
        /// Tries to adds entry to the history. If entry wasn't added (because is whitespace) returns false.
        /// </summary>
        public bool TryAddEntry(string entry)
        {
            string value = entry.TrimEnd('\n', '\r');

            if (string.IsNullOrWhiteSpace(value))
                return false;

            _history.AddLast(value);

            return true;
        }

        /// <summary>
        /// Adds multiple entries to the history.
        /// </summary>
        public void AddEntries(params string[] entries)
        {
            foreach (string entry in entries)
                TryAddEntry(entry);
        }

        /// <summary>
        /// Removes last entry from the history.
        /// </summary>
        public void RemoveLastEntry()
        {
            _history.RemoveLast();
        }

        /// <summary>
        /// Returns history. If <see cref="IsEnabled"/> is set to false, returns empty collection even if history is non-empty.
        /// </summary>
        public IReadOnlyCollection<string> GetEntries()
        {
            return IsEnabled ? _history : new LinkedList<string>();
        }

        /// <summary>
        /// Moves selection up in history list and returns whether selection has changed.
        /// </summary>
        public bool SelectionUp()
        {
            if (_selection is null)
            {
                _selection = _history.Last;
                return true;
            }
            else if (_selection.Previous is LinkedListNode<string> node)
            {
                _selection = node;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Return current selection (empty ehen no selection).
        /// </summary>
        public string GetSelection()
        {
            return _selection?.Value ?? string.Empty;
        }

        /// <summary>
        /// Moves selection down in history list and returns whether selection has changed.
        /// </summary>
        public bool SelectionDown()
        {
            if (_selection?.Next is LinkedListNode<string> node)
            {
                _selection = node;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Resets selection.
        /// </summary>
        public void ResetSelection()
        {
            _selection = null;
        }

        /// <summary>
        /// Clears the history.
        /// </summary>
        public void Clear()
        {
            _history.Clear();
        }
    }
}
