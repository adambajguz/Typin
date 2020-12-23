namespace TypinExamples.Timer.Commands
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Typin;
    using Typin.Attributes;
    using Typin.Console;
    using Typin.Utilities;
    using TypinExamples.Timer.Models;
    using TypinExamples.Timer.Repositories;

    [Command("perf", Description = "Shows performance report.")]
    public class PerformanceCommand : ICommand
    {
        [CommandOption("name", 'n', Description = "Command name to filter.")]
        public string? Name { get; init; }

        private readonly IPerformanceLogsRepository _performanceLogsRepository;

        public PerformanceCommand(IPerformanceLogsRepository performanceLogsRepository)
        {
            _performanceLogsRepository = performanceLogsRepository;
        }

        public async ValueTask ExecuteAsync(IConsole console)
        {
            IEnumerable<PerformanceLog> logs = string.IsNullOrWhiteSpace(Name) ? _performanceLogsRepository.GetAll() : _performanceLogsRepository.GetAll(Name);

            if (logs.Any())
            {
                TableUtils.Write(console,
                                 logs,
                                 new string[] { "Id", "Command name", "Arguments", "Started on", "Time (ms)" },
                                 footnotes: null,
                                 x => x.Id.ToString(),
                                 x => x.CommandName ?? "<default>",
                                 x => '"' + string.Join("\" \"", x.Input!) + '"',
                                 x => x.StartedOn.ToLongTimeString(),
                                 x => x.Time.TotalMilliseconds.ToString());
            }
            else
            {
                await console.Output.WriteLineAsync("Nothing to display.");
            }
        }
    }
}
