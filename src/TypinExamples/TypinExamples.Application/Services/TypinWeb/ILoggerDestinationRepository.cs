namespace TypinExamples.Application.Services.TypinWeb
{
    using System;
    using System.Threading.Tasks;

    public interface ILoggerDestinationRepository : IDisposable
    {
        IWebLoggerDestination Add(string id, IWebLoggerDestination destination);
        void Remove(string id);

        bool Contains(string id);
        IWebLoggerDestination? GetOrDefault(string id);
    }
}