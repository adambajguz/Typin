namespace Typin
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text.RegularExpressions;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;
    using Typin.Console;
    using Typin.Exceptions;
    using Typin.HelpWriter;
    using Typin.Internal;
    using Typin.Internal.DependencyInjection;
    using Typin.Internal.Extensions;
    using Typin.Internal.Pipeline;
    using Typin.Internal.Schemas;
    using Typin.Modes;
    using Typin.OptionFallback;

    /// <summary>
    /// Builds an instance of <see cref="CliApplication"/>.
    /// </summary>
    public sealed class CliApplicationBuilder
    {
        private bool _cliApplicationBuilt;

        //Directives and commands settings
        private readonly List<Type> _commandTypes = new List<Type>();
        private readonly List<Type> _directivesTypes = new List<Type>();

        //Metadata settings
        private string? _title;
        private string? _executableName;
        private string? _versionText;
        private string? _description;
        private string? _startupMessage;

        //Console
        private IConsole? _console;

        //Dependency injection
        private IServiceFactoryAdapter _serviceProviderAdapter = new ServiceFactoryAdapter<IServiceCollection>(new DefaultServiceProviderFactory());
        private readonly List<Action<IServiceCollection>> _configureServicesActions = new List<Action<IServiceCollection>>();
        private readonly List<IConfigureContainerAdapter> _configureContainerActions = new List<IConfigureContainerAdapter>();

        //Modes
        private readonly List<Type> _modeTypes = new List<Type>();
        private Type? _startupMode;

        //Middleware
        private readonly LinkedList<Type> _middlewareTypes = new LinkedList<Type>();

        /// <summary>
        /// Initializes an instance of <see cref="CliApplicationBuilder"/>.
        /// </summary>
        public CliApplicationBuilder()
        {

        }

        #region Directives
        /// <summary>
        /// Add a custom directive to the application.
        /// </summary>
        public CliApplicationBuilder AddDirective(Type directiveType)
        {
            _directivesTypes.Add(directiveType);

            _configureServicesActions.Add(services =>
            {
                services.TryAddTransient(directiveType);
                services.AddTransient(typeof(IDirective), directiveType);
            });

            return this;
        }

        /// <summary>
        /// Add a custom directive to the application.
        /// </summary>
        public CliApplicationBuilder AddDirective<T>()
            where T : IDirective
        {
            return AddDirective(typeof(T));
        }

        /// <summary>
        /// Add custom directives to the application.
        /// </summary>
        public CliApplicationBuilder AddDirectives(IEnumerable<Type> directiveTypes)
        {
            foreach (Type directiveType in directiveTypes)
                AddDirective(directiveType);

            return this;
        }

        /// <summary>
        /// Adds directives from the specified assembly to the application.
        /// Only adds public valid directive types.
        /// </summary>
        public CliApplicationBuilder AddDirectivesFrom(Assembly directiveAssembly)
        {
            foreach (Type directiveType in directiveAssembly.ExportedTypes.Where(SchemasHelpers.IsDirectiveType))
                AddDirective(directiveType);

            return this;
        }

        /// <summary>
        /// Adds directives from the specified assemblies to the application.
        /// Only adds public valid directive types.
        /// </summary>
        public CliApplicationBuilder AddDirectivesFrom(IEnumerable<Assembly> directiveAssemblies)
        {
            foreach (Assembly directiveType in directiveAssemblies)
                AddDirectivesFrom(directiveType);

            return this;
        }

        /// <summary>
        /// Adds directives from the calling assembly to the application.
        /// Only adds public valid directive types.
        /// </summary>
        public CliApplicationBuilder AddDirectivesFromThisAssembly()
        {
            return AddDirectivesFrom(Assembly.GetCallingAssembly());
        }
        #endregion

        #region Commands
        /// <summary>
        /// Adds a command of specified type to the application.
        /// </summary>
        public CliApplicationBuilder AddCommand(Type commandType)
        {
            _commandTypes.Add(commandType);

            _configureServicesActions.Add(services =>
            {
                services.TryAddTransient(commandType);
                services.AddTransient(typeof(ICommand), commandType);
            });

            return this;
        }

        /// <summary>
        /// Adds a command of specified type to the application.
        /// </summary>
        public CliApplicationBuilder AddCommand<T>()
            where T : ICommand
        {
            return AddCommand(typeof(T));
        }

        /// <summary>
        /// Adds multiple commands to the application.
        /// </summary>
        public CliApplicationBuilder AddCommands(IEnumerable<Type> commandTypes)
        {
            foreach (Type commandType in commandTypes)
                AddCommand(commandType);

            return this;
        }

        /// <summary>
        /// Adds commands from the specified assembly to the application.
        /// Only adds public valid command types.
        /// </summary>
        public CliApplicationBuilder AddCommandsFrom(Assembly commandAssembly)
        {
            foreach (Type commandType in commandAssembly.ExportedTypes.Where(SchemasHelpers.IsCommandType))
                AddCommand(commandType);

            return this;
        }

        /// <summary>
        /// Adds commands from the specified assemblies to the application.
        /// Only adds public valid command types.
        /// </summary>
        public CliApplicationBuilder AddCommandsFrom(IEnumerable<Assembly> commandAssemblies)
        {
            foreach (Assembly commandAssembly in commandAssemblies)
                AddCommandsFrom(commandAssembly);

            return this;
        }

        /// <summary>
        /// Adds commands from the calling assembly to the application.
        /// Only adds public valid command types.
        /// </summary>
        public CliApplicationBuilder AddCommandsFromThisAssembly()
        {
            return AddCommandsFrom(Assembly.GetCallingAssembly());
        }
        #endregion

        #region Metadata
        /// <summary>
        /// Sets application title, which appears in the help text.
        /// </summary>
        public CliApplicationBuilder UseTitle(string title)
        {
            _title = title;

            return this;
        }

        /// <summary>
        /// Sets application executable name, which appears in the help text.
        /// </summary>
        public CliApplicationBuilder UseExecutableName(string executableName)
        {
            _executableName = executableName;

            return this;
        }

        /// <summary>
        /// Sets application version text, which appears in the help text and when the user requests version information.
        /// </summary>
        public CliApplicationBuilder UseVersionText(string versionText)
        {
            _versionText = versionText;

            return this;
        }

        /// <summary>
        /// Sets application description, which appears in the help text.
        /// </summary>
        public CliApplicationBuilder UseDescription(string? description)
        {
            _description = description;

            return this;
        }

        /// <summary>
        /// Sets application startup message, which appears just after starting the app.
        ///
        /// You can use the following macros:
        ///     `{title}` for application title,
        ///     `{executable}` for executable name,
        ///     `{version}` for application version,
        ///     `{description}` for application description.
        ///
        /// Double braces can be used to escape macro replacement, while unknown macros will simply act as if they were escaped.
        /// </summary>
        public CliApplicationBuilder UseStartupMessage(string? message)
        {
            _startupMessage = message;

            return this;
        }
        #endregion

        #region Console
        /// <summary>
        /// Configures the application to use the specified implementation of <see cref="IConsole"/>.
        /// </summary>
        public CliApplicationBuilder UseConsole(IConsole console)
        {
            _console = console;

            _configureServicesActions.Add(services =>
            {
                services.AddSingleton<IConsole>(_console);
            });

            return this;
        }

        /// <summary>
        /// Configures the application to use the specified implementation of <see cref="IConsole"/>.
        /// </summary>
        public CliApplicationBuilder UseConsole<T>()
            where T : class, IConsole, new()
        {
            return UseConsole(new T());
        }
        #endregion

        #region Exceptions
        /// <summary>
        /// Configures the application to use the specified implementation of <see cref="ICliExceptionHandler"/>.
        /// Exception handler is configured as scoped service.
        /// </summary>
        public CliApplicationBuilder UseExceptionHandler(Type exceptionHandlerType)
        {
            _configureServicesActions.Add(services =>
            {
                services.AddScoped(typeof(ICliExceptionHandler), exceptionHandlerType);
            });

            return this;
        }

        /// <summary>
        /// Configures the application to use the specified implementation of <see cref="ICliExceptionHandler"/>.
        /// Exception handler is configured as scoped service.
        /// </summary>
        public CliApplicationBuilder UseExceptionHandler<T>()
            where T : class, ICliExceptionHandler
        {
            return UseExceptionHandler(typeof(T));
        }
        #endregion

        #region Modes
        /// <summary>
        /// Registers a CLI mode. Only one mode can be registered as startup mode.
        /// If no mode was registered or none of the registered modes was marked as startup, <see cref="DirectMode"/> will be registered.
        ///
        /// Do not call RegisterMode directly from builder, instead call UseXMode method, e.g. UseDirectMode().
        /// </summary>
        public CliApplicationBuilder RegisterMode<T>(bool asStartup = false)
            where T : ICliMode
        {
            Type cliMode = typeof(T);
            _modeTypes.Add(cliMode);

            _configureServicesActions.Add(services =>
            {
                services.TryAddSingleton(cliMode);
                services.AddSingleton(typeof(ICliMode), (IServiceProvider sp) => sp.GetService(cliMode));
            });

            if (asStartup)
                _startupMode = _startupMode is null ? cliMode : throw new ArgumentException($"Only one mode can be registered as startup mode.", nameof(asStartup));

            return this;
        }
        #endregion

        #region Configuration
        //TODO add configuration builder on actions https://github.com/aspnet/Hosting/blob/f9d145887773e0c650e66165e0c61886153bcc0b/src/Microsoft.Extensions.Hosting/HostBuilder.cs

        /// <summary>
        /// Configures application services.
        /// </summary>
        public CliApplicationBuilder Configure(Action<CliApplicationBuilder> action)
        {
            action.Invoke(this);

            return this;
        }

        /// <summary>
        /// Configures application services.
        /// </summary>
        public CliApplicationBuilder ConfigureServices(Action<IServiceCollection> action)
        {
            _configureServicesActions.Add(action);

            return this;
        }

        /// <summary>
        /// Configures application using <see cref="ICliStartup"/> class instance.
        /// </summary>
        public CliApplicationBuilder UseStartup<T>()
            where T : class, ICliStartup, new()
        {
            ICliStartup t = new T();
            _configureServicesActions.Add(t.ConfigureServices);
            t.Configure(this);

            return this;
        }


        //https://github.com/aspnet/Hosting/blob/f9d145887773e0c650e66165e0c61886153bcc0b/src/Microsoft.Extensions.Hosting/HostBuilder.cs

        /// <summary>
        /// Overrides the factory used to create the service provider.
        /// </summary>
        public CliApplicationBuilder UseServiceProviderFactory<TContainerBuilder>(IServiceProviderFactory<TContainerBuilder> factory)
        {
            _serviceProviderAdapter = new ServiceFactoryAdapter<TContainerBuilder>(factory ?? throw new ArgumentNullException(nameof(factory)));

            return this;
        }

        /// <summary>
        /// Enables configuring the instantiated dependency container. This can be called multiple times and
        /// the results will be additive.
        /// </summary>
        public CliApplicationBuilder ConfigureContainer<TContainerBuilder>(Action<TContainerBuilder> configureDelegate)
        {
            _configureContainerActions.Add(new ConfigureContainerAdapter<TContainerBuilder>(configureDelegate
                ?? throw new ArgumentNullException(nameof(configureDelegate))));

            return this;
        }
        #endregion

        #region Middleware
        /// <summary>
        /// Adds a middleware to the command execution pipeline.
        /// Middlewares are also registered as scoped services and are executed in registration order.
        /// </summary>
        public CliApplicationBuilder UseMiddleware(Type middleware)
        {
            _configureServicesActions.Add(services =>
            {
                services.AddScoped(typeof(IMiddleware), middleware);
                services.AddScoped(middleware);
            });

            _middlewareTypes.AddFirst(middleware);

            return this;
        }

        /// <summary>
        /// Adds a middleware to the command execution pipeline.
        /// </summary>
        public CliApplicationBuilder UseMiddleware<TMiddleware>()
            where TMiddleware : class
        {
            return UseMiddleware(typeof(TMiddleware));
        }
        #endregion

        #region Value fallback
        /// <summary>
        /// Configures to use a specific option fallback provider with desired lifetime <see cref="EnvironmentVariableFallbackProvider"/>.
        /// </summary>
        public CliApplicationBuilder UseOptionFallbackProvider(Type fallbackProviderType, ServiceLifetime lifetime = ServiceLifetime.Singleton)
        {
            _configureServicesActions.Add(services =>
            {
                services.Add(new ServiceDescriptor(typeof(IOptionFallbackProvider), fallbackProviderType, lifetime));
            });

            return this;
        }

        /// <summary>
        /// Configures to use a specific option fallback provider with desired lifetime <see cref="EnvironmentVariableFallbackProvider"/>.
        /// </summary>
        public CliApplicationBuilder UseOptionFallbackProvider<T>(ServiceLifetime lifetime = ServiceLifetime.Singleton)
            where T : IOptionFallbackProvider
        {
            return UseOptionFallbackProvider(typeof(T), lifetime);
        }
        #endregion

        #region Help Writer
        /// <summary>
        /// Configures to use a specific help writer with transient lifetime.
        /// </summary>
        public CliApplicationBuilder UseHelpWriter(Type helpWriterType)
        {
            _configureServicesActions.Add(services =>
            {
                services.AddTransient(typeof(IHelpWriter), helpWriterType);
            });

            return this;
        }

        /// <summary>
        /// Configures to use a specific help writer with transient lifetime.
        /// </summary>
        public CliApplicationBuilder UseHelpWriter<T>()
            where T : IHelpWriter
        {
            return UseOptionFallbackProvider(typeof(T));
        }
        #endregion

        /// <summary>
        /// Creates an instance of <see cref="CliApplication"/> using configured parameters.
        /// Default values are used in place of parameters that were not specified.
        /// A scope is defined as a lifetime of a command execution pipeline that includes directives handling.
        /// </summary>
        public CliApplication Build()
        {
            if (_cliApplicationBuilt)
                throw new InvalidOperationException("Build can only be called once.");

            _cliApplicationBuilt = true;

            // Set default values
            _title ??= AssemblyExtensions.TryGetDefaultTitle() ?? "App";
            _executableName ??= AssemblyExtensions.TryGetDefaultExecutableName() ?? "app";
            _versionText ??= AssemblyExtensions.TryGetDefaultVersionText() ?? "v1.0";
            _console ??= new SystemConsole();

            if (_startupMode is null || _modeTypes.Count == 0)
                this.UseDirectMode(true);

            // Format startup message
            if (_startupMessage != null)
            {
                _startupMessage = Regex.Replace(_startupMessage, @"{(?<x>[^}]+)}", match =>
                {
                    string value = match.Groups["x"].Value;

                    return value.ToLower() switch
                    {
                        "title" => _title,
                        "executable" => _executableName,
                        "version" => _versionText,
                        "description" => _description ?? string.Empty,
                        _ => string.Concat("{", value, "}")
                    };
                });
            }

            // Add core middlewares to the end of the pipeline
            AddCoreMiddlewares();

            // Create context
            var _serviceCollection = new ServiceCollection();

            var metadata = new ApplicationMetadata(_title, _executableName, _versionText, _description, _startupMessage);
            var configuration = new ApplicationConfiguration(_modeTypes,
                                                             _commandTypes,
                                                             _directivesTypes,
                                                             _middlewareTypes,
                                                             _startupMode!,
                                                             _serviceCollection);

            var cliContextFactory = new CliContextFactory(metadata, configuration, _console);

            // Add core services
            _serviceCollection.AddOptions();
            _serviceCollection.AddSingleton(typeof(ApplicationMetadata), metadata);
            _serviceCollection.AddSingleton(typeof(ApplicationConfiguration), configuration);
            _serviceCollection.AddSingleton(typeof(IConsole), _console);
            _serviceCollection.AddScoped(typeof(ICliContext), (provider) => cliContextFactory.Create(provider));
            _serviceCollection.AddSingleton<ICliCommandExecutor, CliCommandExecutor>();
            _serviceCollection.AddSingleton<ICliApplicationLifetime, CliApplicationLifetime>();

            IServiceProvider serviceProvider = CreateServiceProvider(_serviceCollection);

            return new CliApplication(serviceProvider, cliContextFactory);
        }

        private void AddCoreMiddlewares()
        {
            UseMiddleware<ResolveCommandSchema>();
            UseMiddleware<HandleVersionOption>();
            UseMiddleware<ResolveCommandInstance>();
            UseMiddleware<HandleHelpOption>();
            UseMiddleware<ExecuteCommand>();
        }

        private IServiceProvider CreateServiceProvider(ServiceCollection services)
        {
            foreach (Action<IServiceCollection> configureServicesAction in _configureServicesActions)
            {
                configureServicesAction(services);
            }

            services.TryAddSingleton<IOptionFallbackProvider, EnvironmentVariableFallbackProvider>();
            services.TryAddScoped<ICliExceptionHandler, DefaultExceptionHandler>();
            services.TryAddScoped<IHelpWriter, DefaultHelpWriter>();

            object? containerBuilder = _serviceProviderAdapter.CreateBuilder(services);
            foreach (IConfigureContainerAdapter containerAction in _configureContainerActions)
            {
                containerAction.ConfigureContainer(containerBuilder);
            }

            IServiceProvider? appServices = _serviceProviderAdapter.CreateServiceProvider(containerBuilder);

            return appServices ?? throw new InvalidOperationException($"{nameof(IServiceFactoryAdapter)} returned a null instance of object implementing {nameof(IServiceProvider)}.");
        }
    }
}