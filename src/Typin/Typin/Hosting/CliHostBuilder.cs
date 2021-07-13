namespace Typin.Hosting
{
    using System;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Typin.Hosting.Startup;

    internal class CliHostBuilder : ICliHostBuilder
    {
        private readonly CliHostBuilderOptions _options;
        private object? _startupObject;

        /// <inheritdoc/>
        public IHostBuilder HostBuilder { get; }

        public CliHostBuilder(IHostBuilder builder, CliHostBuilderOptions cliHostBuilderOptions)
        {
            HostBuilder = builder;
            _options = cliHostBuilderOptions;

            builder.ConfigureServices((context, services) =>
            {
                services.Configure<CliHostServiceOptions>(options =>
                {

                });

                //// Register startup input
                //StartupInputProvider defaultInputProvider = new(cliHostBuilderOptions.CommandLineOverride, cliHostBuilderOptions.CommandLineOverrideStartsWithExecutableName);
                //services.AddInputProvider<IStartupInputProvider>(defaultInputProvider);

                //// Register environemnt variables accessor
                //EnvironmentVariablesAccessor environmentVariablesAccessor = new(cliHostBuilderOptions.EnvironmentVariablesOverride);
                //services.AddSingleton<IEnvironmentVariablesAccessor>(environmentVariablesAccessor);

                services.AddSingleton<CliApplication>();
                //services.AddSingleton<IRootSchemaAccessor, RootSchemaAccessor>();
                //services.AddSingleton<ICliCommandExecutor, CliCommandExecutor>();
                //services.AddSingleton<MiddlewarePipelineProvider>();
                //services.AddSingleton<ICliApplicationLifetime, CliApplicationLifetime>();

                //services.TryAddSingleton<IApplicationBuilderFactory, ApplicationBuilderFactory>();

                //services.TryAddSingleton<IOptionFallbackProvider, EnvironmentVariableFallbackProvider>();
                //services.TryAddScoped<ICliExceptionHandler, DefaultExceptionHandler>();
                //services.TryAddScoped<IHelpWriter, DefaultHelpWriter>();

                //services.AddScoped(typeof(ICliContext), (provider) =>
                //{
                //    IRootSchemaAccessor rootSchemaAccessor = provider.GetRequiredService<IRootSchemaAccessor>();
                //    ApplicationMetadata metadata = provider.GetRequiredService<ApplicationMetadata>();
                //    ApplicationConfiguration configuration = provider.GetRequiredService<ApplicationConfiguration>();
                //    IConsole console = provider.GetRequiredService<IConsole>();

                //    return new CliContext(metadata,
                //                          configuration,
                //                          rootSchemaAccessor.RootSchema,
                //                          null!);
                //});
            });
        }

        /// <inheritdoc/>
        public ICliHostBuilder Configure(Action<IApplicationBuilder> configure)
        {
            // Clear the startup type
            _startupObject = configure;

            HostBuilder.ConfigureServices((context, services) =>
            {
                if (object.ReferenceEquals(_startupObject, configure))
                {
                    services.Configure<CliHostServiceOptions>(options =>
                    {
                        options.ConfigureApplication = configure;
                    });
                }
            });

            return this;
        }

        /// <inheritdoc/>
        public ICliHostBuilder UseStartup<TStartup>()
            where TStartup : notnull, IStartup, new()
        {
            IStartup startup = new TStartup();

            HostBuilder.ConfigureServices(startup.ConfigureServices);
            Configure(startup.Configure);

            return this;
        }

        /// <inheritdoc/>
        public ICliHostBuilder UseStartup<TStartup, TContainerBuilder>()
            where TStartup : IStartup<TContainerBuilder>, new()
            where TContainerBuilder : notnull
        {
            IStartup<TContainerBuilder> startup = new TStartup();

            HostBuilder.ConfigureServices(startup.ConfigureServices);
            HostBuilder.ConfigureContainer<TContainerBuilder>(startup.ConfigureContainer);
            Configure(startup.Configure);

            return this;
        }
    }
}
