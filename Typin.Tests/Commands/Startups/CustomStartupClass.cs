namespace Typin.Tests.Commands.Startups
{
    using Microsoft.Extensions.DependencyInjection;
    using Typin.Console;
    using Typin.Directives;
    using Typin.Tests.Commands.Valid;

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

            app.AddCommand<DefaultCommand>()
               .AddCommandsFrom(typeof(DefaultCommand).Assembly)
               .AddCommands(new[] { typeof(DefaultCommand) })
               .AddCommandsFrom(new[] { typeof(DefaultCommand).Assembly })
               .AddCommandsFromThisAssembly()
               .AddDirective<DebugDirective>()
               .AddDirective<PreviewDirective>()
               .AddDirective<CustomInteractiveModeOnlyDirective>()
               .AddDirective<CustomDirective>()
               .UseTitle("test")
               .UseExecutableName("test")
               .UseVersionText("test")
               .UseDescription("test")
               .UseConsole(console);
        }
    }
}
