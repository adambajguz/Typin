namespace TypinExamples.HelloWorld
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Typin;
    using Typin.Directives;
    using TypinExamples.HelloWorld.Commands;

    public static class Program
    {

        public static async Task<int> Main()
        {
            return await new CliApplicationBuilder().AddCommand<SimpleCommand>()
                                                    .AddDirective<DebugDirective>()
                                                    .AddDirective<PreviewDirective>()
                                                    .Build()
                                                    .RunAsync();
        }
    }
}
