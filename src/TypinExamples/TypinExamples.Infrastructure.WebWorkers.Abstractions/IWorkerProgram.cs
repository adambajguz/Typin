namespace TypinExamples.Infrastructure.WebWorkers.Abstractions
{
    using System.Threading.Tasks;

    public interface IWorkerProgram
    {
        Task<int> Main();
    }
}
