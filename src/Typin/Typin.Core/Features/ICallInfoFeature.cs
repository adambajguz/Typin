namespace Typin.Features
{
    using System;

    /// <summary>
    /// Command line call informations feature.
    /// </summary>
    public interface ICallInfoFeature
    {
        /// <summary>
        /// Context instance id.
        /// </summary>
        Guid Identifier { get; }

        /// <summary>
        /// Context trace identifier meeting the format "{<see cref="ContextDepth"/>}:{<see cref="Identifier"/>}:{<see cref="ParentContext"/>?Call.Identifier ?? "root"}".
        /// </summary>
        string TraceIdentifier { get; }

        /// <summary>
        /// Current <see cref="CliContext"/> instance.
        /// </summary>
        CliContext? CurrentContext { get; }

        /// <summary>
        /// Parent <see cref="CliContext"/> instance.
        /// </summary>
        CliContext? ParentContext { get; }

        /// <summary>
        /// CLI context depth.
        /// </summary>
        int ContextDepth { get; }

        /// <summary>
        /// Call start timestamp.
        /// </summary>
        DateTimeOffset StartedAt { get; }

        /// <summary>
        /// Gets the total elapsed time of the call.
        /// </summary>
        TimeSpan Elapsed { get; }

        /// <summary>
        /// Gets the total elapsed time of the call, in milliseconds.
        /// </summary>
        long ElapsedMilliseconds { get; }

        /// <summary>
        /// Gets the total elapsed time of the call, in timer ticks.
        /// </summary>
        long ElapsedTicks { get; }
    }
}
