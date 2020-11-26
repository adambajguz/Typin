namespace TypinExamples.Infrastructure.WebWorkers.Abstractions
{
    public interface IWorkerConfigurationBuilder
    {
        IWorkerConfigurationBuilder UseProgram<T>(bool asDefault = true)
            where T : class, IWorkerProgram;
    }
}
