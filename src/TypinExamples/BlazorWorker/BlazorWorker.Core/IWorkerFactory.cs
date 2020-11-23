namespace BlazorWorker.Core
{
    using System.Threading.Tasks;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public interface IWorkerFactory
    {
        Task<IWorker> CreateAsync();
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
