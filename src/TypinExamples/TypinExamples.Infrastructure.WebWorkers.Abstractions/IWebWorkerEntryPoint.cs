namespace TypinExamples.Infrastructure.WebWorkers.Abstractions
{
    using System.Threading.Tasks;

    public interface IWebWorkerEntryPoint
    {
        Task<int> Main();
    }
}
