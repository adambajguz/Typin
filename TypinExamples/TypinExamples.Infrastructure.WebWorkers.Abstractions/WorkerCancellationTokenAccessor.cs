namespace TypinExamples.Infrastructure.WebWorkers.Abstractions
{
    using System.Threading;

    public sealed class WorkerCancellationTokenAccessor
    {
        public CancellationToken Token { get; }

        public WorkerCancellationTokenAccessor(CancellationToken token)
        {
            Token = token;
        }
    }
}
