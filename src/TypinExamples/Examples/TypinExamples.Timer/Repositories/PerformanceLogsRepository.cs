namespace TypinExamples.Timer.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Typin;
    using Typin.Schemas;
    using TypinExamples.Timer.Models;

    public class PerformanceLogsRepository : IPerformanceLogsRepository
    {
        private readonly List<PerformanceLog> _performanceLogs = new();

        private readonly ICliContext _cliContext;

        public PerformanceLogsRepository(ICliContext cliContext)
        {
            _cliContext = cliContext;
        }

        public IEnumerable<PerformanceLog> GetAll()
        {
            return _performanceLogs;
        }

        public IEnumerable<PerformanceLog> GetAll(string name)
        {
            CommandSchema? commandSchema = _cliContext.RootSchema.TryFindCommand(name);

            if (commandSchema is not null)
                return _performanceLogs.Where(x => x.CommandName == commandSchema.Name);

            return Array.Empty<PerformanceLog>();
        }

        public void Insert(PerformanceLog entry)
        {
            _performanceLogs.Add(entry with { Id = _performanceLogs.Count });
        }

        public void DeleteById(int id)
        {
            _performanceLogs.RemoveAll(x => x.Id == id);
        }

        public void DeleteAll()
        {
            _performanceLogs.Clear();
        }

        public void DeleteAll(string name)
        {
            CommandSchema? commandSchema = _cliContext.RootSchema.TryFindCommand(name);

            if (commandSchema is not null)
                _performanceLogs.RemoveAll(x => x.CommandName == commandSchema.Name);
        }
    }
}
