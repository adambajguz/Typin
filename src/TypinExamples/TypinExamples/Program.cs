namespace TypinExamples
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
    using TypinExamples.Application;
    using TypinExamples.Infrastructure.Compiler;
    using TypinExamples.Infrastructure.TypinWeb;
    using TypinExamples.Infrastructure.Workers;

    public static class Program
    {
        public static async Task Main(string[] args)
        {
            WebAssemblyHostBuilder builder = WebAssemblyHostBuilder.CreateDefault(args);
            WebAssemblyHostConfiguration configuration = builder.Configuration;
            IWebAssemblyHostEnvironment hostEnvironment = builder.HostEnvironment;

            builder.RootComponents.Add<App>("app");

            builder.Services.ConfigureServices(configuration, hostEnvironment)
                            .ConfigureApplicationServices(configuration)
                            .ConfigureInfrastructureWorkerServices(configuration)
                            .ConfigureInfrastructureTypinWebServices()
                            .ConfigureInfrastructureCompilerServices();

            await builder.Build().RunAsync();
        }
    }
}
