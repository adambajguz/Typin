namespace TypinExamples.Infrastructure.WebWorkers.Hil
{
    using System;
    using System.Threading.Tasks;
    using TypinExamples.Infrastructure.WebWorkers.WorkerCore;

    public interface IWorkerBackgroundService<T> : IAsyncDisposable where T : class
    {
        /// <summary>
        /// Queues the specified work to run on the underlying worker and returns a <see cref="Task"/> object that represents that work.
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="function"></param>
        /// <returns></returns>
        Task<int> RunAsync();

        /// <summary>
        /// Returns the message service used by the underlying worker.
        /// </summary>
        /// <returns></returns>
        IWorkerMessageService GetWorkerMessageService();
    }
}
