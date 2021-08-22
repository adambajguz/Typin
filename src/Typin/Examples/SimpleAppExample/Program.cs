namespace SimpleAppExample
{
    using System.Threading.Tasks;
    using Microsoft.Extensions.Hosting;
    using Typin;
    using Typin.Commands;
    using Typin.Hosting;

    public static class Program
    {
        private static readonly string[] Arguments = { "-125", "--str", "hello world", "-i", "-13", "-b", "-vx" };

        public static async Task<int> Main()
        {
            await CliHost.CreateDefaultBuilder()
                .ConfigureCliHost((cliBuilder) =>
                {
                    cliBuilder.GetOrAddComponentScanner<ICommand>(
                        services =>
                        {
                            return new CommandComponentScanner(services);
                        },
                        scanner =>
                        {
                            scanner.FromThisAssembly();
                        });
                })
                .RunConsoleAsync();

            return 0;
            //return await new CliApplicationBuilder().AddCommand<SampleCommand>()
            //                                        .AddDirective<DebugDirective>()
            //                                        .AddDirective<PreviewDirective>()
            //                                        .Build()
            //                                        .RunAsync(Arguments, new Dictionary<string, string>());
        }
    }
}