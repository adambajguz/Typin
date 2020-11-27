namespace TypinExamples.Infrastructure.WebWorkers.WorkerCore.Internal
{
    using System;
    using TypinExamples.Infrastructure.WebWorkers.Hil;

    public static class WorkerProgram
    {
        private static WorkerInstanceManager? _workerInstance;

        public static void Main(ulong workerId)
        {
#if DEBUG
            Console.WriteLine($"{nameof(WorkerInstanceManager)}.Main({workerId})");
#endif

            _workerInstance = new(workerId, new DefaultSerializer());

#if DEBUG
            Console.WriteLine($"{nameof(WorkerInstanceManager)}.Main({workerId}): Done.");
#endif
        }
    }
}
