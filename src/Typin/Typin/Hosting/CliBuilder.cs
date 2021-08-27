﻿namespace Typin.Hosting
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
    using Typin.Exceptions;
    using Typin.Help;
    using Typin.Internal;
    using Typin.Internal.Components;
    using Typin.Internal.DynamicCommands;

    internal class CliBuilder : ICliBuilder, IDisposable
    {
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

            services.AddOptions();

            services.AddOptions<ApplicationMetadata>()
                    .PostConfigure(options =>
                    {
                        options.Title ??= AssemblyUtils.TryGetDefaultTitle() ?? "App";
                        options.ExecutableName ??= AssemblyUtils.TryGetDefaultExecutableName() ?? "app";
                        options.VersionText ??= AssemblyUtils.TryGetDefaultVersionText() ?? "v1.0";
                    });

            services.AddOptions<CliOptions>()
                    .PostConfigure(options =>
                    {
                        options.CommandLine ??= System.Environment.CommandLine;
                        options.CommandLineStartsWithExecutableName = true;
                    });

            services.AddScoped<ICliContextAccessor, CliContextAccessor>();
            services.AddSingleton<IRootSchemaAccessor, RootSchemaAccessor>();
            services.AddSingleton<ICliCommandExecutor, CliCommandExecutor>();
            services.AddSingleton<IDynamicCommandBuilderFactory, DynamicCommandBuilderFactory>();
        }

        /// <inheritdoc/>
        public ICliBuilder GetOrAddScanner<TComponent>(Func<IServiceCollection, IScanner<TComponent>> factory, Action<IScanner<TComponent>> scanner)
            where TComponent : class
        {
            if (!_components.TryGetValue(typeof(TComponent), out IScanner? componentScanner))
            {
                componentScanner = factory(Services);
                _components.Add(typeof(TComponent), componentScanner);
            }

            IScanner<TComponent> genericComponentScanner = (componentScanner as IScanner<TComponent>)!;
            scanner(genericComponentScanner);

            return this;
        }

        /// <inheritdoc/>
        public ICliBuilder GetOrAddScanner<TComponent>(Func<ICliBuilder, IScanner<TComponent>> factory, Action<IScanner<TComponent>> scanner)
            where TComponent : class
        {
            if (_components.TryGetValue(typeof(TComponent), out IScanner? componentScanner))
            {
                componentScanner = factory(this);
                _components.Add(typeof(TComponent), componentScanner);
            }

            IScanner<TComponent> genericComponentScanner = (componentScanner as IScanner<TComponent>)!;
            scanner(genericComponentScanner);

            return this;
        }

        /// <summary>
        /// Finalizes <see cref="CliBuilder"/>.
        /// </summary>
        public void Dispose()
        {
            Dictionary<Type, IReadOnlyList<Type>> cliComponents = _components.ToDictionary(x => x.Key, x => x.Value.Types);
            ComponentProvider cliComponentProvider = new(cliComponents);

            Services.AddSingleton<IComponentProvider>(cliComponentProvider);
            Services.TryAddSingleton<IConsole, SystemConsole>();

            Services.TryAddScoped<ICliExceptionHandler, DefaultExceptionHandler>();
            Services.TryAddScoped<IHelpWriter, DefaultHelpWriter>();

            Services.AddPipelining(builder =>
            {
                builder.AddInitializer<MiddlewaresInitailizer>();
            });

            Services.AddHostedService<CliHostService>();
        }
    }
}
