namespace TypinExamples.TypinWeb.Commands
{
    using Typin;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using Typin.Attributes;
    using Typin.Console;
    using Typin.Utilities;

    [Command("services", Description = "Prints a list of registered services in application.")]
    public class ServicesCommand : ICommand
    {
        private readonly ApplicationConfiguration _configuration;

        public ServicesCommand(ApplicationConfiguration configuration)
        {
            _configuration = configuration;
        }

        public ValueTask ExecuteAsync(IConsole console)
        {
            DebugPrintServices(console, _configuration.Services);

            return default;
        }

        private static void DebugPrintServices(IConsole console, IEnumerable<ServiceDescriptor> serviceDescriptors)
        {
            TableUtils.Write(console,
                             serviceDescriptors.OrderBy(x => x.Lifetime)
                                               .ThenBy(x => x.ServiceType.Name)
                                               .ThenBy(x => x.ImplementationType?.Name)
                                               .GroupBy(x => x.Lifetime),
                             new string[] { "Service type", "Implementation type", "F", "I", "Lifetime" },
                             footnotes:
                             "  F - whether implementation factory is used\n" +
                             "  I - whether implementation instanace is used",
                             x => x.ServiceType.Name,
                             x => x.ImplementationType == null ? string.Empty : x.ImplementationType.Name,
                             x => x.ImplementationFactory == null ? string.Empty : "+",
                             x => x.ImplementationInstance == null ? string.Empty : "+",
                             x => x.Lifetime.ToString());
        }
    }
}
