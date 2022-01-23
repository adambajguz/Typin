namespace Typin.Features
{
    using System;
    using System.Threading;

    /// <summary>
    /// <see cref="ICallLifetimeFeature"/> implementation.
    /// </summary>
    public class CallLifetimeFeature : ICallLifetimeFeature
    {
        private readonly CancellationTokenSource _cancellationTokenSource;

        /// <summary>
        /// Initializes a new instance of <see cref="CallLifetimeFeature"/>;
        /// </summary>
        /// <param name="cancellationTokenSource"></param>
        public CallLifetimeFeature(CancellationTokenSource cancellationTokenSource)
        {
            _cancellationTokenSource = cancellationTokenSource;
            CallAborted = cancellationTokenSource.Token;
        }

        /// <inheritdoc/>
        public CancellationToken CallAborted { get; set; }

        void ICallLifetimeFeature.Abort()
        {
            _cancellationTokenSource.Cancel();
        }

        void ICallLifetimeFeature.AbortAfter(TimeSpan timeSpan)
        {
            _cancellationTokenSource.CancelAfter(timeSpan);
        }

        /// <inheritdoc/>
        public override string? ToString()
        {
            return base.ToString() +
                " | " +
                $"{nameof(CallAborted)} = {CallAborted.IsCancellationRequested}";
        }
    }
}
