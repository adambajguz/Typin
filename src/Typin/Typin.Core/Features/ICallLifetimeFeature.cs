namespace Typin.Features
{
    using System;
    using System.Threading;

    /// <summary>
    /// Provides access to the command line call lifetime operations.
    /// </summary>
    public interface ICallLifetimeFeature
    {
        /// <summary>
        /// A <see cref="CancellationToken"/> that fires if the call is aborted and
        /// the application should cease processing.
        /// The token will not fire if the request completes successfully.
        /// </summary>
        CancellationToken CallAborted { get; set; }

        /// <summary>
        /// Forcefully aborts the request if it has not already completed.
        /// This will result in <see cref="CallAborted"/> being triggered.
        /// </summary>
        void Abort();

        /// <summary>
        /// Forcefully aborts the request if it has not already completed within specified time.
        /// This will result in <see cref="CallAborted"/> being triggered.
        /// </summary>
        void AbortAfter(TimeSpan timeSpan);
    }
}
