namespace TypinExamples.Infrastructure.WebWorkers.Abstractions
{
    public interface IWorkerConfigurationBuilder
    {
        /// <summary>
        /// Use specific worker program class.
        /// </summary>
        IWorkerConfigurationBuilder UseProgram<TProgram>()
            where TProgram : class, IWorkerProgram;

        /// <summary>
        /// Use long running cancellable worker program class that will return 0 when it finishes.
        /// This can be used when you don't have any long running logic that you need to call and you only rely on messages.
        /// </summary>
        IWorkerConfigurationBuilder UseLongRunningProgram();
    }
}
