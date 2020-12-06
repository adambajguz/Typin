namespace TypinExamples.Services.Terminal
{
    using System.Collections.Generic;
    using TypinExamples.Application.Services.TypinWeb;

    public sealed class LoggerDestinationRepository : ILoggerDestinationRepository
    {
        private static readonly Dictionary<string, IWebLoggerDestination> _destinations = new Dictionary<string, IWebLoggerDestination>();

        public LoggerDestinationRepository()
        {

        }

        public IWebLoggerDestination Add(string id, IWebLoggerDestination destination)
        {
            _destinations.TryAdd(id, destination);

            return destination;
        }

        public void Remove(string id)
        {
            _destinations.Remove(id);
        }

        public bool Contains(string id)
        {
            return _destinations.ContainsKey(id);
        }

        public IWebLoggerDestination? GetOrDefault(string id)
        {
            _destinations.TryGetValue(id, out IWebLoggerDestination? term);

            return term;
        }

        public void Dispose()
        {
            _destinations.Clear();
        }
    }
}