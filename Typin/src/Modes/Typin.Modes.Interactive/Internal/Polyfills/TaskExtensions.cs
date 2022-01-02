#if !NET6_0_OR_GREATER
namespace System.Threading.Tasks
{
    using System.Threading;

    /// <summary>
    /// https://stackoverflow.com/questions/28626575/can-i-cancel-streamreader-readlineasync-with-a-cancellationtoken
    /// </summary>
    internal static class TaskExtensions
    {
        public static async Task<T> WaitAsync<T>(this Task<T> task, CancellationToken cancellationToken)
        {
            using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            Task waiting = Task.Delay(-1, cts.Token);

            await Task.WhenAny(waiting, task);
            cts.Cancel();
            cancellationToken.ThrowIfCancellationRequested();

            return await task;
        }
    }
}
#endif
