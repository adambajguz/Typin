namespace Typin.Tests.Data.Startups
{
    using Microsoft.Extensions.DependencyInjection;
    using Typin.Console;
    using Typin.Directives;
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
