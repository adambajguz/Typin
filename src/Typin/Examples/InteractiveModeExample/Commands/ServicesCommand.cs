﻿//namespace InteractiveModeExample.Commands
//{
//    using System.Collections.Generic;
//    using System.Linq;
//    using System.Threading;
//    using System.Threading.Tasks;
//    using Microsoft.Extensions.DependencyInjection;
//    using Typin;
//    using Typin.Attributes;
//    using Typin.Console;
//    using Typin.Utilities;

//    [Command("services", Description = "Prints a list of registered services in application.")]
//    public class ServicesCommand : ICommand
//    {
//        private readonly CliContext _cliContext;
//        private readonly IConsole _console;

//        public ServicesCommand(CliContext cliContext, IConsole console)
//        {
//            _cliContext = cliContext;
//            _console = console;
//        }

//        public ValueTask ExecuteAsync(CancellationToken cancellationToken)
//        {
//            DebugPrintServices(_console, _cliContext.Configuration.Services);

//            return default;
//        }

//        private static void DebugPrintServices(IConsole console, IEnumerable<ServiceDescriptor> serviceDescriptors)
//        {
//            TableUtils.Write(console.Output,
//                             serviceDescriptors.OrderBy(x => x.Lifetime)
//                                               .ThenBy(x => x.ServiceType.Name)
//                                               .ThenBy(x => x.ImplementationType?.Name)
//                                               .GroupBy(x => x.Lifetime),
//                             new[] { "Service type", "Implementation type", "F", "I", "Lifetime" },
//                             footnotes:
//                             "  F - whether implementation factory is used\n" +
//                             "  I - whether implementation instanace is used",
//                             x => x.ServiceType.Name,
//                             x => x.ImplementationType == null ? string.Empty : x.ImplementationType.Name,
//                             x => x.ImplementationFactory == null ? string.Empty : "+",
//                             x => x.ImplementationInstance == null ? string.Empty : "+",
//                             x => x.Lifetime.ToString());
//        }
//    }
//}
