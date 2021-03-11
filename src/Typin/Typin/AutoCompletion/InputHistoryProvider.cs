namespace Typin.AutoCompletion
{
    using System.Collections.Generic;

    /// <summary>
    /// Provides command input history in interactive mode.
    /// </summary>
    public class InputHistoryProvider : IInputHistoryProvider
    {
        private readonly LinkedList<string> _history = new();
        private LinkedListNode<string>? _selection;

        /// <inheritdoc/>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// Initializes an instance of <see cref="InputHistoryProvider"/>.
        /// </summary>
        internal InputHistoryProvider()
        {

        }

        /// <inheritdoc/>
        public bool TryAddEntry(string entry)
        {
            string value = entry.TrimEnd('\n', '\r');

            if (string.IsNullOrWhiteSpace(value))
            {
                return false;
            }

            _history.AddLast(value);

            return true;
        }

        /// <inheritdoc/>
        public void AddEntries(params string[] entries)
        {
            foreach (string entry in entries)
            {
                TryAddEntry(entry);
            }
        }

        /// <inheritdoc/>
        public void RemoveLastEntry()
        {
            _history.RemoveLast();
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public void Clear()
        {
            _history.Clear();
        }
    }
}
