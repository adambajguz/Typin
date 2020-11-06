namespace Typin.InteractiveModeDemo.Commands
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
        private readonly ICliContext _cliContext;

        public PipelineCommand(ICliContext cliContext)
        {
            _cliContext = cliContext;
        }

        public ValueTask ExecuteAsync(IConsole console)
        {
            DebugPrintServices(console, _cliContext.Configuration.MiddlewareTypes);

            return default;
        }

        private void DebugPrintServices(IConsole console, IReadOnlyCollection<Type> middlewares)
        {
            TableUtils.Write(console,
                             middlewares.Reverse()
                                        .Concat(new Type?[] { null })
                                        .Concat(middlewares),
                             new string[] { "Middleware type name", "Assembly" },
                             footnotes: null,
                             x => x == null ? "<PipelineTermination>" : (x.FullName == null ? string.Empty : x.FullName.ToString()),
                             x => x == null ? string.Empty : x.Assembly.ToString());
        }
    }
}
