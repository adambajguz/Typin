namespace Typin.Tests.Data.Commands.Valid
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Typin.Attributes;
    using Typin.Console;
    using Typin.Utilities;

    [Command("pipeline", Description = "Prints a middleware pipeline structure in application.")]
    public class PipelineCommand : ICommand
    {
        public const string PipelineTermination = "<PipelineTermination>";
        private readonly ICliContext _cliContext;

        public PipelineCommand(ICliContext cliContext)
        {
            _cliContext = cliContext;
        }

        public ValueTask ExecuteAsync(IConsole console)
        {
            DebugPrintPipeline(console, _cliContext.Configuration.MiddlewareTypes);

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
