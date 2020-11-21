namespace TypinExamples.Core.Services
{
    using System.Threading.Tasks;
    using TypinExamples.Domain.Models;

    public interface ICoreMessageDispatcher
    {
        Task DispatchAsync(WorkerMessage model);
    }
}