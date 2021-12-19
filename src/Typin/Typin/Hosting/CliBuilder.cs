namespace Typin.Hosting
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;
    using Microsoft.Extensions.Hosting;
    using PackSite.Library.Pipelining;
    using Typin.Components;
    using Typin.Console;
    using Typin.DynamicCommands;
    using Typin.Help;
    using Typin.Internal;
    using Typin.Internal.Components;
    using Typin.Internal.DynamicCommands;

    internal class CliBuilder : ICliBuilder, IDisposable
    {
        private readonly bool _subsequentCall;
        private readonly Dictionary<Type, IScanner> _components = new();

        /// <inheritdoc/>
        public HostBuilderContext Context { get; }

        /// <inheritdoc/>
        public IHostEnvironment Environment { get; }

        /// <inheritdoc/>
        public IConfiguration Configuration { get; }

        /// <inheritdoc/>
        public IServiceCollection Services { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="CliBuilder"/>.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="services"></param>
        public CliBuilder(HostBuilderContext context, IServiceCollection services)
        {
            Context = context;
            Environment = context.HostingEnvironment;
            Configuration = context.Configuration;
            Services = services;

            if (services.Any(x => x.ServiceType == typeof(CliModeManager)))
            {
                _subsequentCall = true;
                return;
            }

            services.AddOptions();

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
                            options.StartupExecutionOptions = CommandExecutionOptions.TrimExecutable;
                        }
                    });

            services.AddSingleton<ICliContextAccessor, CliContextAccessor>();
            services.AddSingleton<IRootSchemaAccessor, RootSchemaAccessor>();
            services.AddSingleton<ICommandExecutor, CommandExecutor>();
            services.AddSingleton<CliModeManager>();
            services.AddSingleton<ICliModeSwitcher>((provider) => provider.GetRequiredService<CliModeManager>());
            services.AddSingleton<ICliModeAccessor>((provider) => provider.GetRequiredService<CliModeManager>());

            services.AddSingleton<IDynamicCommandBuilderFactory, DynamicCommandBuilderFactory>();
        }

        /// <inheritdoc/>
        public ICliBuilder GetOrAddScanner<TComponent, TInterface>(Func<ICliBuilder, TInterface> factory, Action<TInterface> scanner)
            where TComponent : class
            where TInterface : class, IScanner<TComponent>
        {
            if (!_components.TryGetValue(typeof(TComponent), out IScanner? componentScanner))
            {
                componentScanner = factory(this);
                _components.Add(typeof(TComponent), componentScanner);
            }

            TInterface genericComponentScanner = (componentScanner as TInterface)!;
            scanner(genericComponentScanner);

            return this;
        }

        /// <summary>
        /// Finalizes <see cref="CliBuilder"/>.
        /// </summary>
        public void Dispose()
        {
            if (_subsequentCall && _components.Count <= 0)
            {
                return;
            }

            Dictionary<Type, IReadOnlyCollection<Type>> cliComponents = _components.ToDictionary(x => x.Key, x => x.Value.Types);
            ComponentProvider cliComponentProvider = new(cliComponents);

            if (_subsequentCall)
            {
                ServiceDescriptor sd = Services.Where(x => x.ServiceType == typeof(IComponentProvider)).First();
                Services.RemoveAll<IComponentProvider>();

                ComponentProvider prev = sd.ImplementationInstance as ComponentProvider ?? throw new NullReferenceException($"Invalid {typeof(ComponentProvider)}");
                ComponentProvider n = prev.Merge(cliComponentProvider);

                Services.AddSingleton<IComponentProvider>(n);

                return;
            }

            Services.AddSingleton<IComponentProvider>(cliComponentProvider);
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
