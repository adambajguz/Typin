﻿namespace TypinExamples.HelloWorld
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Typin;
    using Typin.Directives;
    using TypinExamples.Infrastructure.TypinWeb.Configuration;
    using TypinExamples.TypinWeb.Extensions;

    public static class WebProgram
    {
        public static async Task<int> WebMain(WebCliConfiguration configuration, string commandLine, IReadOnlyDictionary<string, string> environmentVariables)
        {
            return await new CliApplicationBuilder().AddCommandsFromThisAssembly()
                                                    .AddDirective<PreviewDirective>()
                                                    .UseWebExample(configuration)
                                                    .Build()
                                                    .RunAsync(commandLine, environmentVariables, true);
        }
    }
}
