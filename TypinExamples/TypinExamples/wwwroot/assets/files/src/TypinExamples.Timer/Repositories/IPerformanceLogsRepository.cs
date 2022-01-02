namespace TypinExamples.Timer.Repositories
{
    using System.Collections.Generic;
    using TypinExamples.Timer.Models;

    public interface IPerformanceLogsRepository
    {
        IEnumerable<PerformanceLog> GetAll();

        IEnumerable<PerformanceLog> GetAll(string name);

        void DeleteById(int id);
        void DeleteAll();
        void DeleteAll(string name);

        void Insert(PerformanceLog entry);
    }
}
