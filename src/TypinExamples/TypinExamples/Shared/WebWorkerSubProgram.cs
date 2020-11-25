namespace TypinExamples.Shared
{
    using System;
    using System.Threading.Tasks;
    using TypinExamples.Infrastructure.WebWorkers.Abstractions;

    public class WebWorkerSubProgram : IWebWorkerEntryPoint
    {
        public Task<int> Main()
        {
            return Task.FromResult(new Random().Next());
        }
    }
}
