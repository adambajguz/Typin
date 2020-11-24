namespace BlazorWorker.Core.CoreInstanceService
{
    using BlazorWorker.Core.SimpleInstanceService;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public static class SimpleInstanceServiceExtension
    {
        public static ICoreInstanceService CreateCoreInstanceService(this IWorker source)
        {
            return new CoreInstanceService(new SimpleInstanceServiceProxy(source));
        }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
