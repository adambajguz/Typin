﻿//namespace InteractiveModeExample.Commands
//{
//    using System;
//    using System.Collections.Generic;
//    using System.Linq;
//    using System.Threading;
//    using System.Threading.Tasks;
//    using PackSite.Library.Pipelining;
//    using Typin;
//    using Typin.Models.Attributes; using Typin.Commands.Attributes;
//    using Typin.Console;
//    using Typin.Utilities;

//    [Command("pipeline", Description = "Prints a middleware pipeline structure in application.")]
//    public class PipelineCommand : ICommand
//    {
//        public const string PipelineTermination = "<PipelineTermination>";

//        private readonly IPipelineCollection _pipelineCollection;
//        private readonly IConsole _console;

//        public PipelineCommand(IPipelineCollection pipelineCollection, IConsole console)
//        {
//            _pipelineCollection = pipelineCollection;
//            _console = console;
//        }

//        public ValueTask ExecuteAsync(CancellationToken cancellationToken)
//        {
//            IPipeline<ICliContext> pipeline = _pipelineCollection.Get<ICliContext>();
//            DebugPrintPipeline(_console, pipeline.Steps);

//            return default;
//        }

//        private static void DebugPrintPipeline(IConsole console, IReadOnlyCollection<Type> middlewares)
//        {
//            TableUtils.Write(console.Output,
//                             middlewares
//                                        .Concat(new Type?[] { null })
//                                        .Concat(middlewares.Reverse()),
//                             new[] { "Middleware type name", "Assembly" },
//                             footnotes: null,
//                             x => x == null ? PipelineTermination : x.FullName == null ? string.Empty : x.FullName.ToString(),
//                             x => x == null ? string.Empty : x.Assembly.ToString());
//        }
//    }
//}