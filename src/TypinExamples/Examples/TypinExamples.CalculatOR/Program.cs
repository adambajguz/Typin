namespace TypinExamples.CalculatOR
{
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using Typin;
    using Typin.Directives;
    using TypinExamples.CalculatOR.Services;
    using TypinExamples.TypinWeb.Commands;

    public static class Program
    {
        public static async Task<int> Main()
        {
            return await new CliApplicationBuilder().AddCommandsFromThisAssembly()
                                                    .AddCommand<ServicesCommand>()
                                                    .AddDirective<PreviewDirective>()
                                                    .ConfigureServices((services) => services.AddSingleton<OperationEvaluatorService>())
                                                    .Build()
                                                    .RunAsync();
        }
    }
}
