namespace TypinExamples.Application.Services.Workers
{
    using System.Threading.Tasks;
    using TypinExamples.Domain.Models.Workers;

    public interface ICoreMessageDispatcher
    {
        Task DispatchAsync(WorkerMessage model);
    }
}