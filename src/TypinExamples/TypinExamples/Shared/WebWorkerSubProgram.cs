namespace TypinExamples.Shared
{
    using System.Threading.Tasks;
    using TypinExamples.Infrastructure.WebWorkers.Abstractions;

    public class WebWorkerSubProgram : IWebWorkerEntryPoint
    {
        public Task<int> Main()
        {
            return Task.FromResult(893428489);
        }
    }
}
