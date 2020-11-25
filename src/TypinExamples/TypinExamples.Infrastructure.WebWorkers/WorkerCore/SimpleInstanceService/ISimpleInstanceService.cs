namespace TypinExamples.Infrastructure.WebWorkers.WorkerCore.SimpleInstanceService
{
    using System.Threading.Tasks;

    public interface ISimpleInstanceService
    {
        Task<DisposeResult> DisposeInstance(DisposeInstanceRequest request);
        Task<InitInstanceResult> InitInstance(InitInstanceRequest initInstanceRequest);
    }
}