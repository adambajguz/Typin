namespace Typin
{
    /// <summary>
    /// Provides methods to switch and get current mode.
    /// </summary>
    public sealed class CliModeSwitcher
    {
        private readonly object _lock = new object();
        private readonly CliContext _cliContext;

        /// <summary>
        /// Current mode.
        /// </summary>
        public CliModes Current { get; private set; }

        /// <summary>
        /// Mode to switch to or null if nothing needs to be changes.
        /// </summary>
        public CliModes? Pending { get; private set; }

        /// <summary>
        /// Whether mode switch was queued.
        /// </summary>
        public bool IsPending => Pending is null;

        /// <summary>
        /// Initializes an instance of <see cref="CliModeSwitcher"/>.
        /// </summary>
        internal CliModeSwitcher(CliContext cliContext)
        {
            _cliContext = cliContext;
        }

        /// <summary>
        /// Queues mode switch when mode differs from current.
        /// </summary>
        public void Queue(CliModes mode)
        {
            lock (_lock)
            {
                if (mode != Current)
                {
                    Pending = mode;
                }
            }
        }

        internal void Switch()
        {
            lock (_lock)
            {
                if (Pending is CliModes cm)
                {
                    Current = cm;
                }
            }
        }
    }
}
