namespace TypinExamples.Validation
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Typin;
    using TypinExamples.Infrastructure.TypinWeb.Configuration;
    using TypinExamples.TypinWeb.Extensions;

    public static class WenProgram
    {
        public static async Task<int> WebMain(WebCliConfiguration configuration, string commandLine, IReadOnlyDictionary<string, string> environmentVariables)
        {
            return await new CliApplicationBuilder().UseStartup<Startup>()
                                                    .UseWebExample(configuration)
                                                    .Build()
                                                    .RunAsync(commandLine, environmentVariables, true);
        }
    }
}
