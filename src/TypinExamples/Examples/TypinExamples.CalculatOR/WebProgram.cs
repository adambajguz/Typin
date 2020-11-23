namespace TypinExamples.CalculatOR
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using Typin;
    using Typin.Directives;
    using TypinExamples.CalculatOR.Services;
    using TypinExamples.TypinWeb.Configuration;
    using TypinExamples.TypinWeb.Extensions;

    public static class WebProgram
    {
        public static async Task<int> WebMain(WebCliConfiguration configuration, string commandLine, IReadOnlyDictionary<string, string> environmentVariables)
        {
            return await new CliApplicationBuilder().AddCommandsFromThisAssembly()
                                                    .AddDirective<PreviewDirective>()
                                                    .ConfigureServices((services) => services.AddSingleton<OperationEvaluatorService>())
                                                    .UseWebExample(configuration)
                                                    .Build()
                                                    .RunAsync(commandLine, environmentVariables, true);
        }
    }
}
