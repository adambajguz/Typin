namespace TypinExamples.Infrastructure.WebWorkers.WorkerCore
{
    using System.Threading;
    using System.Threading.Tasks;
    using TypinExamples.Infrastructure.WebWorkers.Abstractions;

    internal class LongRunningWorkerProgram : IWorkerProgram
    {
        public async Task<int> Main(CancellationToken cancellationToken)
        {
            try
            {
                await Task.Delay(Timeout.Infinite, cancellationToken);
            }
            catch (TaskCanceledException)
            {

            }

            return 0;
        }
    }
}
