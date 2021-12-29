namespace Typin.Hosting
{
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;
    using Microsoft.Extensions.Hosting;
    using PackSite.Library.Pipelining;
    using Typin;
    using Typin.Console;
    using Typin.Help;
    using Typin.Internal;
    using Typin.Modes;

    internal class DefaultCliBuilder : CliBuilder
    {
        /// <summary>
        /// Initializes a new instance of <see cref="DefaultCliBuilder"/>.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="services"></param>
        public DefaultCliBuilder(HostBuilderContext context, IServiceCollection services) :
            base(context, services)
        {
            if (!SubsequentCall)
            {
                services.AddOptions<ApplicationMetadata>()
                        .PostConfigure(options =>
                        {
                            options.Title ??= Environment.ApplicationName;
                            options.ExecutableName ??= AssemblyUtils.TryGetDefaultExecutableName() ?? "app";
                            options.VersionText ??= AssemblyUtils.GetDefaultVersionText();
                        });

                services.AddOptions<CliOptions>()
                        .PostConfigure(options =>
                {
                    if (options.CommandLine is null && options.CommandLineArguments is null)
                    {
                        options.CommandLine = System.Environment.CommandLine;
                        options.StartupInputOptions = InputOptions.TrimExecutable;
                    }
                });

                services.AddSingleton<ICliContextAccessor, CliContextAccessor>();
                services.AddSingleton<IRootSchemaAccessor, RootSchemaAccessor>();
                services.AddSingleton<ICommandExecutor, CommandExecutor>();
                services.AddSingleton<CliModeManager>();
                services.AddSingleton<ICliModeSwitcher>((provider) => provider.GetRequiredService<CliModeManager>());
                services.AddSingleton<ICliModeAccessor>((provider) => provider.GetRequiredService<CliModeManager>());

                //services.AddSingleton<IDynamicCommandBuilderFactory, DynamicCommandBuilderFactory>();
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (SubsequentCall || !disposing)
            {
                return;
            }

            Services.TryAddSingleton<IConsole, SystemConsole>();
            Services.TryAddScoped<IHelpWriter, DefaultHelpWriter>();

            Services.AddPipelining(builder =>
            {
                builder.AddInitializer<MiddlewaresInitializer>();
            });

            Services.AddHostedService<CliHostService>();
        }
    }
}
