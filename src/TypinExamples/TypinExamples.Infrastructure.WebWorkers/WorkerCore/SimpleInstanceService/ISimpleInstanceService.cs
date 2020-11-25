namespace TypinExamples.Infrastructure.WebWorkers.WorkerCore.SimpleInstanceService
{
    using System.Threading.Tasks;
    using TypinExamples.Infrastructure.WebWorkers.WorkerCore.SimpleInstanceService.Messages;

    public interface ISimpleInstanceService
    {
        Task<DisposeResult> DisposeInstance(DisposeInstanceRequest request);
        Task<InitInstanceResult> InitInstance(InitInstanceRequest initInstanceRequest);
    }
}