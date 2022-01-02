namespace TypinExamples.Application.Services.TypinWeb
{
    using System;
    using System.Threading.Tasks;
    using TypinExamples.Infrastructure.WebWorkers.Abstractions;

    public interface ITerminalRepository : IDisposable
    {
        Task<IWebTerminal> CreateTerminalAsync(string id, string exampleKey, IWorker worker);
        Task UnregisterAndDisposeTerminalAsync(string id);

        bool Contains(string id);
        IWebTerminal? GetOrDefault(string id);
    }
}