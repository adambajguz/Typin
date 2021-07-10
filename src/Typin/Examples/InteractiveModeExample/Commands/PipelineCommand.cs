namespace InteractiveModeExample.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Typin;
    using Typin.Attributes;
    using Typin.Console;
    using Typin.Utilities;

    [Command("pipeline", Description = "Prints a middleware pipeline structure in application.")]
    public class PipelineCommand : ICommand
    {
        private readonly ApplicationConfiguration _configuration;
        private readonly IConsole _console;

        public PipelineCommand(ApplicationConfiguration configuration, IConsole console)
        {
            _configuration = configuration;
            _console = console;
        }

        public ValueTask ExecuteAsync(CancellationToken cancellationToken)
        {
            DebugPrintPipeline(_console, _configuration.MiddlewareTypes);

            return default;
        }

        private static void DebugPrintPipeline(IConsole console, IReadOnlyCollection<Type> middlewares)
        {
            TableUtils.Write(console.Output,
                             middlewares.Concat(new Type?[] { null })
                                        .Concat(middlewares.Reverse()),
                             new[] { "Middleware type name", "Assembly" },
                             footnotes: null,
                             x => x == null ? "<PipelineTermination>" : x.FullName == null ? string.Empty : x.FullName.ToString(),
                             x => x == null ? string.Empty : x.Assembly.ToString());
        }
    }
}
