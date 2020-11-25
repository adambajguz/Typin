namespace TypinExamples.Infrastructure.WebWorkers.Core.CoreInstanceService
{
    using TypinExamples.Infrastructure.WebWorkers.Abstractions;
    using TypinExamples.Infrastructure.WebWorkers.Core.SimpleInstanceService;

    public static class SimpleInstanceServiceExtension
    {
        public static ICoreInstanceService CreateCoreInstanceService(this IWorker source)
        {
            return new CoreInstanceService(new SimpleInstanceServiceProxy(source));
        }
    }
}
