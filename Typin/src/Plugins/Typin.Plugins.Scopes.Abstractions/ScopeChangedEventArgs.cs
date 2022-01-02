namespace Typin.Plugins.Scopes
{
    using System;

    /// <summary>
    /// Arguments of the event invoked when a scope changed.
    /// </summary>
    public sealed class ScopeChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Previos scope.
        /// </summary>
        public string Previous { get; }

        /// <summary>
        /// Current scope.
        /// </summary>
        public string Current { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="ScopeChangedEventArgs"/>.
        /// </summary>
        /// <param name="previous"></param>
        /// <param name="current"></param>
        public ScopeChangedEventArgs(string previous, string current)
        {
            Previous = previous ?? throw new ArgumentNullException(nameof(previous));
            Current = current ?? throw new ArgumentNullException(nameof(current));
        }
    }
}