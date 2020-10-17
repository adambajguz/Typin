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
            DebugPrintServices(console, _cliContext.Middlewares);

            return default;
        }

        private void DebugPrintServices(IConsole console, IReadOnlyCollection<Type> middlewares)
        {
            TableUtils.Write(console,
                             middlewares.Reverse()
                                        .Concat(new Type?[] { null })
                                        .Concat(middlewares),
                             new string[] { "AssemblyQualifiedName" },
                             footnotes: null,
                             x => x == null ? PipelineTermination : x.AssemblyQualifiedName == null ? string.Empty : x.AssemblyQualifiedName.ToString());
        }
    }
}
