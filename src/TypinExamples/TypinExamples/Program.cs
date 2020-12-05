namespace TypinExamples
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
    using Serilog;
    using Serilog.Events;
    using Serilog.Exceptions;
    using TypinExamples.Application;
    using TypinExamples.Infrastructure.Compiler;

    public static class Program
    {
        public static async Task Main(string[] args)
        {
            WebAssemblyHostBuilder builder = WebAssemblyHostBuilder.CreateDefault(args);
            WebAssemblyHostConfiguration configuration = builder.Configuration;
            IWebAssemblyHostEnvironment hostEnvironment = builder.HostEnvironment;

            builder.RootComponents.Add<App>("app");

            AddSerilog(hostEnvironment);

            builder.Services.ConfigureServices(configuration, hostEnvironment)
                            .ConfigureApplicationServices(configuration)
                            .ConfigureInfrastructureCompilerServices();

            await builder.Build().RunAsync();
        }

        private static void AddSerilog(IWebAssemblyHostEnvironment hostEnvironment)
        {
            var loggerConfiguration = new LoggerConfiguration()
                .WriteTo.BrowserConsole()
                .Enrich.FromLogContext()
                .Enrich.WithExceptionDetails();

            if (hostEnvironment.IsDevelopment())
            {
                loggerConfiguration.MinimumLevel.Override("Microsoft", LogEventLevel.Verbose)
                                   .MinimumLevel.Verbose();
            }
            else
            {
                loggerConfiguration.MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                               .MinimumLevel.Information();
            }

            Log.Logger = loggerConfiguration.CreateLogger();
            Log.Logger.Information("Initializing {Program}", typeof(Program).FullName);
        }
    }
}
