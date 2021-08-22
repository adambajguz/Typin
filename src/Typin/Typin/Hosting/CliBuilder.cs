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
    using Typin.Exceptions;
    using Typin.Help;
    using Typin.Internal;
    using Typin.Internal.Components;
    using Typin.Internal.DynamicCommands;
    using Typin.OptionFallback;

    internal class CliBuilder : ICliBuilder, IDisposable
    {
        private readonly Dictionary<Type, IComponentScanner> _components = new();

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


            ApplicationMetadata metadata = new(string.Empty, string.Empty, string.Empty, string.Empty);

            EnvironmentVariablesAccessor environmentVariablesAccessor = new();

            // Add core services
            services.AddOptions();
            services.AddPipelining();
            services.AddSingleton(typeof(ApplicationMetadata), metadata);
            services.AddSingleton(typeof(IConsole), new SystemConsole());
            services.AddSingleton(typeof(IEnvironmentVariablesAccessor), environmentVariablesAccessor);
            services.AddSingleton<IRootSchemaAccessor, RootSchemaAccessor>();
            services.AddSingleton<ICliCommandExecutor, CliCommandExecutor>();
            services.AddSingleton<ICliApplicationLifetime, CliApplicationLifetime>();
            services.AddSingleton<IDynamicCommandBuilderFactory, DynamicCommandBuilderFactory>();
        }

        /// <inheritdoc/>
        public ICliBuilder GetOrAddComponentScanner<TComponent>(Func<IServiceCollection, IComponentScanner<TComponent>> factory, Action<IComponentScanner<TComponent>> builder)
            where TComponent : class
        {
            if (!_components.TryGetValue(typeof(TComponent), out IComponentScanner? scanner))
            {
                scanner = factory(Services);
                _components.Add(typeof(TComponent), scanner);
            }

            IComponentScanner<TComponent> componentScanner = (scanner as IComponentScanner<TComponent>)!;
            builder(componentScanner);

            return this;
        }

        /// <inheritdoc/>
        public ICliBuilder GetOrAddComponentScanner<TComponent>(Func<ICliBuilder, IComponentScanner<TComponent>> factory, Action<IComponentScanner<TComponent>> builder)
            where TComponent : class
        {
            if (_components.TryGetValue(typeof(TComponent), out IComponentScanner? scanner))
            {
                scanner = factory(this);
                _components.Add(typeof(TComponent), scanner);
            }

            IComponentScanner<TComponent> componentScanner = (scanner as IComponentScanner<TComponent>)!;
            builder(componentScanner);

            return this;
        }

        /// <summary>
        /// Finalizes <see cref="CliBuilder"/>.
        /// </summary>
        public void Dispose()
        {
            Dictionary<Type, IReadOnlyList<Type>> cliComponents = _components.ToDictionary(x => x.Key, x => x.Value.Types);
            CliComponentProvider cliComponentProvider = new(cliComponents);

            Services.AddSingleton(cliComponentProvider);

            Services.TryAddSingleton<IOptionFallbackProvider, EnvironmentVariableFallbackProvider>();
            Services.TryAddScoped<ICliExceptionHandler, DefaultExceptionHandler>();
            Services.TryAddScoped<IHelpWriter, DefaultHelpWriter>();

            Services.AddHostedService<CliHostService>();
        }
    }
}
