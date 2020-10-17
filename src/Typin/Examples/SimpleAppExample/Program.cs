namespace SimpleAppExample
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using SimpleAppExample.Commands;
    using Typin;
    using Typin.Directives;

    public static class Program
    {
        private static readonly string[] Arguments = { "-1", "--str", "hello world", "-i", "-13", "-b", "-vx" };

        public static async Task<int> Main()
        {
            return await new CliApplicationBuilder().AddCommand<SampleCommand>()
                                                    .AddDirective<DebugDirective>()
                                                    .AddDirective<PreviewDirective>()
                                                    .Build()
                                                    .RunAsync(Arguments, new Dictionary<string, string>());
        }
    }
}