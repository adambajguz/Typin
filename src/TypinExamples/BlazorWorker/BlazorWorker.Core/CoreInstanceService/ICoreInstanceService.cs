namespace BlazorWorker.Core.CoreInstanceService
{
    using System;
    using System.Threading.Tasks;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public interface ICoreInstanceService
    {
        Task<IInstanceHandle> CreateInstance(Type type);
        Task<IInstanceHandle> CreateInstance<T>();

        Task<IInstanceHandle> CreateInstance<T>(Action<WorkerInitOptions> options);

        Task<IInstanceHandle> CreateInstance<T>(WorkerInitOptions options);
    }

    public interface IInstanceHandle : IAsyncDisposable
    {
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
