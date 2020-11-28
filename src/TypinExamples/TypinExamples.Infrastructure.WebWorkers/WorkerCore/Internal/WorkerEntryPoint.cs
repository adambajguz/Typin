namespace TypinExamples.Infrastructure.WebWorkers.WorkerCore.Internal
{
    using System;
    using TypinExamples.Infrastructure.WebWorkers.Common.Messaging;
    using TypinExamples.Infrastructure.WebWorkers.WorkerCore;

    internal static class WorkerEntryPoint
    {
        private static WorkerInstanceManager? _workerInstance;

        public static void Init(ulong workerId, string? startupType)
        {
#if DEBUG
            Console.WriteLine($"{nameof(WorkerEntryPoint)}.Main({workerId}, {startupType})");
#endif

            _workerInstance = new(workerId, new DefaultSerializer());
            _workerInstance.Start(startupType);

#if DEBUG
            Console.WriteLine($"{nameof(WorkerEntryPoint)}.Main({workerId}, {startupType}): Done.");
#endif
        }
    }
}
