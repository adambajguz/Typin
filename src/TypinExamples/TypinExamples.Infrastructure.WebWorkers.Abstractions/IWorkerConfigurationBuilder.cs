namespace TypinExamples.Infrastructure.WebWorkers.Abstractions
{
    public interface IWorkerConfigurationBuilder
    {
        IWorkerConfigurationBuilder UseProgram<TProgram>(bool asDefault = true)
            where TProgram : class, IWorkerProgram;
    }
}
