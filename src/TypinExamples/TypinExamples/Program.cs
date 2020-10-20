namespace TypinExamples
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
    using TypinExamples.Compiler.Services;

    public static class Program
    {
        public static async Task Main(string[] args)
        {
            WebAssemblyHostBuilder builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            builder.ConfigureServices()
                   .ConfigureCompilerServices();

            await builder.Build().RunAsync();
        }
    }
}
