namespace TypinExamples
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
    using TypinExamples.Compiler.Services;
    using TypinExamples.Application;
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
                            .ConfigureCoreServices(configuration)
                            .ConfigureWorkersLayer(configuration)
                            .ConfigureCompilerServices();

            await builder.Build().RunAsync();
        }
    }
}
