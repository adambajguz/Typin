namespace Typin.Tests.Data.Commands.Valid
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using PackSite.Library.Pipelining;
    using Typin.Attributes;
    using Typin.Console;
    using Typin.Utilities;

    [Command("pipeline", Description = "Prints a middleware pipeline structure in application.")]
    public class PipelineCommand : ICommand
    {
        public const string PipelineTermination = "<PipelineTermination>";

        private readonly IPipelineCollection _pipelineCollection;
        private readonly IConsole _console;

        public PipelineCommand(IPipelineCollection pipelineCollection, IConsole console)
        {
            _pipelineCollection = pipelineCollection;
            _console = console;
        }

        public ValueTask ExecuteAsync(CancellationToken cancellationToken)
        {
            IPipeline<ICliContext> pipeline = _pipelineCollection.Get<ICliContext>();
            DebugPrintPipeline(_console, pipeline.Steps);

            return default;
        }

        private static void DebugPrintPipeline(IConsole console, IReadOnlyCollection<Type> middlewares)
        {
            TableUtils.Write(console.Output,
                             middlewares.Reverse()
                                        .Concat(new Type?[] { null })
                                        .Concat(middlewares),
                             new string[] { "AssemblyQualifiedName" },
                             footnotes: null,
                             x => x == null ? PipelineTermination : x.AssemblyQualifiedName == null ? string.Empty : x.AssemblyQualifiedName.ToString());
        }
    }
}
