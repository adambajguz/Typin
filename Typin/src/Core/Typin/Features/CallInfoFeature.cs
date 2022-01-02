namespace Typin.Features
{
    using System;
    using System.Diagnostics;
    using Typin;

    /// <summary>
    /// <see cref="ICallInfoFeature"/> implementation.
    /// </summary>
    internal sealed class CallInfoFeature : ICallInfoFeature
    {
        private readonly Stopwatch _stopwatch = Stopwatch.StartNew();

        /// <inheritdoc/>
        public Guid Identifier { get; }

        /// <inheritdoc/>
        public string TraceIdentifier { get; }

        /// <inheritdoc/>
        public CliContext CurrentContext { get; }

        /// <inheritdoc/>
        public CliContext? ParentContext { get; }

        /// <inheritdoc/>
        public int ContextDepth { get; }

        /// <inheritdoc/>
        public DateTimeOffset StartedAt { get; }

        /// <inheritdoc/>
        public TimeSpan Elapsed => _stopwatch.Elapsed;

        /// <inheritdoc/>
        public long ElapsedMilliseconds => _stopwatch.ElapsedMilliseconds;

        /// <inheritdoc/>
        public long ElapsedTicks => _stopwatch.ElapsedTicks;

        /// <summary>
        /// Initializes a new instance of <see cref="CallInfoFeature"/>.
        /// </summary>
        public CallInfoFeature(Guid identifier, CliContext current, CliContext? parent)
        {
            Identifier = identifier;
            CurrentContext = current;
            ParentContext = parent;
            ContextDepth = parent is null ? 0 : parent.Call.ContextDepth + 1;

            StartedAt = DateTimeOffset.UtcNow;

            TraceIdentifier = $"{ContextDepth}:{Identifier}:{ParentContext?.Call.Identifier.ToString() ?? "root"}";
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return base.ToString() +
                " | " +
                $"{nameof(TraceIdentifier)} = {TraceIdentifier}";
        }
    }
}
