namespace Typin.Tests.Data.Startups
{
    using Microsoft.Extensions.DependencyInjection;
    using Typin.Console;
    using Typin.Directives;
    using Typin.Modes;
    using Typin.Tests.Data.Commands.Valid;
    using Typin.Tests.Data.CustomDirectives.Valid;
    using Typin.Tests.Data.Services;

    public class CustomStartupClass : ICliStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<DependencyA>();
            services.AddScoped<DependencyB>();
            services.AddTransient<DependencyC>();
        }

        public void Configure(CliApplicationBuilder app)
        {
            var (console, _, _) = VirtualConsole.CreateBuffered();

            app.AddCommand<BenchmarkDefaultCommand>()
               .AddCommandsFrom(typeof(BenchmarkDefaultCommand).Assembly)
               .AddCommands(new[] { typeof(BenchmarkDefaultCommand) })
               .AddCommandsFrom(new[] { typeof(BenchmarkDefaultCommand).Assembly })
               .AddCommandsFromThisAssembly()
               .AddDirective<DebugDirective>()
               .AddDirective<PreviewDirective>()
               .AddDirective<CustomInteractiveModeOnlyDirective>()
               .AddDirective<CustomDirective>()
               .UseTitle("test")
               .UseExecutableName("test")
               .UseVersionText("test")
               .UseDescription("test")
               .UseConsole(console)
               .UseDirectMode(true)
               .UseInteractiveMode();
        }
    }
}
