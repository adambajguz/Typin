namespace Typin.Internal.Extensions
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// https://stackoverflow.com/questions/28626575/can-i-cancel-streamreader-readlineasync-with-a-cancellationtoken
    /// </summary>
    internal static class TaskExtensions
    {
        public static async Task<T> WithCancellation<T>(this Task<T> task, CancellationToken cancellationToken)
        {
            using (var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken))
            {
                var waiting = Task.Delay(-1, cts.Token);

                await Task.WhenAny(waiting, task);
                cts.Cancel();
                cancellationToken.ThrowIfCancellationRequested();

                return await task;
            }
        }

        public static async Task<T> WithCancellation<T>(this Task<T> task, CancellationToken cancellationToken, Action abortAction, bool useSynchronizationContext = false)
        {
            using (cancellationToken.Register(abortAction, useSynchronizationContext))
            {
                try
                {
                    return await task;
                }
                catch (Exception ex)
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        // the Exception will be available as Exception.InnerException
                        throw new OperationCanceledException(ex.Message, ex, cancellationToken);
                    }

                    // cancellation hasn't been requested, rethrow the original Exception
                    throw;
                }
            }
        }
    }
}
