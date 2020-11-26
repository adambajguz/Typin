namespace TypinExamples.Infrastructure.WebWorkers.Core.Internal
{
    internal sealed class WorkerIdProvider
    {
        private ulong _id;

        public WorkerIdProvider()
        {

        }

        public ulong Next()
        {
            return _id++;
        }
    }
}
