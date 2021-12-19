namespace Typin
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;
    using Microsoft.Extensions.Logging;
    using Typin.Console;
    using Typin.Exceptions;
    using Typin.Help;
    using Typin.Internal;
    using Typin.Internal.DependencyInjection;
    using Typin.Internal.Pipeline;
    using Typin.Modes;
    using Typin.OptionFallback;
    using Typin.Schemas;

    /// <summary>
    /// Builds an instance of <see cref="CliApplication"/>.
    /// </summary>
    public sealed class CliApplicationBuilder
    {
        private bool _cliApplicationBuilt;

        //Directives and commands settings
        private readonly List<Type> _commandTypes = new();
        private readonly List<Type> _directivesTypes = new();

        //Metadata settings
        private string? _title;
        private string? _executableName;
        private string? _versionText;
        private string? _description;
        private Action<ApplicationMetadata, IConsole>? _startupMessage;

        //Console
        private IConsole? _console;
        private bool _isSelfCreatedConsoleInstance;

        //Dependency injection
        private IServiceFactoryAdapter _serviceProviderAdapter = new ServiceFactoryAdapter<IServiceCollection>(new DefaultServiceProviderFactory());
        private readonly List<Action<IServiceCollection>> _configureServicesActions = new();
        private readonly List<IConfigureContainerAdapter> _configureContainerActions = new();

        //Modes
        private readonly List<Type> _modeTypes = new();
        private Type? _startupMode;

        //Middleware
        private readonly LinkedList<Type> _middlewareTypes = new();

        /// <summary>
        /// Initializes an instance of <see cref="CliApplicationBuilder"/>.
        /// </summary>
        public CliApplicationBuilder()
        {
            this.AddBeforeUserMiddlewares();
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
            {
                AddDirective(directiveType);
            }

            return this;
        }

        /// <summary>
        /// Adds directives from the specified assembly to the application.
        /// Only adds public valid directive types.
        /// </summary>
        public CliApplicationBuilder AddDirectivesFrom(Assembly directiveAssembly)
        {
            foreach (Type directiveType in directiveAssembly.ExportedTypes.Where(KnownTypesHelpers.IsDirectiveType))
            {
                AddDirective(directiveType);
            }

            return this;
        }

        /// <summary>
        /// Adds directives from the specified assemblies to the application.
        /// Only adds public valid directive types.
        /// </summary>
        public CliApplicationBuilder AddDirectivesFrom(IEnumerable<Assembly> directiveAssemblies)
        {
            foreach (Assembly directiveType in directiveAssemblies)
            {
                AddDirectivesFrom(directiveType);
            }

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
            {
                AddCommand(commandType);
            }

            return this;
        }

        /// <summary>
        /// Adds commands from the specified assembly to the application.
        /// Only adds public valid command types.
        /// </summary>
        public CliApplicationBuilder AddCommandsFrom(Assembly commandAssembly)
        {
            foreach (Type commandType in commandAssembly.ExportedTypes.Where(KnownTypesHelpers.IsCommandType))
            {
                AddCommand(commandType);
            }

            return this;
        }

        /// <summary>
        /// Adds commands from the specified assemblies to the application.
        /// Only adds public valid command types.
        /// </summary>
        public CliApplicationBuilder AddCommandsFrom(IEnumerable<Assembly> commandAssemblies)
        {
            foreach (Assembly commandAssembly in commandAssemblies)
            {
                AddCommandsFrom(commandAssembly);
            }

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
        #endregion

        #region Startup message
        /// <summary>
        /// Sets application startup message, which appears just after starting the app.
        /// </summary>
        public CliApplicationBuilder UseStartupMessage(string message, ConsoleColor messageColor = ConsoleColor.DarkYellow)
        {
            _startupMessage = (metadata, console) =>
            {
                console.Output.WithForegroundColor(messageColor, (output) => output.WriteLine(message));
            };

            return this;
        }

        /// <summary>
        /// Sets application startup message, which appears just after starting the app.
        /// </summary>
        public CliApplicationBuilder UseStartupMessage(Func<ApplicationMetadata, string> message, ConsoleColor messageColor = ConsoleColor.DarkYellow)
        {
            _startupMessage = (metadata, console) =>
            {
                string tmp = message(metadata);

                console.Output.WithForegroundColor(messageColor, (output) => output.WriteLine(tmp));
            };

            return this;
        }

        /// <summary>
        /// Sets application startup message, which appears just after starting the app.
        /// </summary>
        public CliApplicationBuilder UseStartupMessage(Action<ApplicationMetadata, IConsole> message)
        {
            _startupMessage = message;

            return this;
        }
        #endregion

        #region Console
        /// <summary>
        /// Configures the application to use the specified implementation of <see cref="IConsole"/>.
        /// Console will be automatically diposed before exiting from <see cref="CliApplication.RunAsync(IEnumerable{string}, IReadOnlyDictionary{string, string})"/>.
        /// </summary>
        public CliApplicationBuilder UseConsole(IConsole console)
        {
            _console = console;
            _isSelfCreatedConsoleInstance = false;

            return this;
        }

        /// <summary>
        /// Configures the application to use the specified implementation of <see cref="IConsole"/>.
        /// </summary>
        public CliApplicationBuilder UseConsole<T>()
            where T : class, IConsole, new()
        {
            _console = new T();
            _isSelfCreatedConsoleInstance = true;

            return this;
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
            return RegisterMode(typeof(T), asStartup);
        }

        /// <summary>
        /// Registers a CLI mode. Only one mode can be registered as startup mode.
        /// If no mode was registered or none of the registered modes was marked as startup, <see cref="DirectMode"/> will be registered.
        ///
        /// Do not call RegisterMode directly from builder, instead call UseXMode method, e.g. UseDirectMode().
        /// </summary>
        public CliApplicationBuilder RegisterMode(Type modeType, bool asStartup = false)
        {
            Type cliMode = modeType;
            _modeTypes.Add(cliMode);

            if (!KnownTypesHelpers.IsCliModeType(modeType))
            {
                throw new ArgumentException($"Invalid CLI mode type '{modeType}'.", nameof(modeType));
            }

            _configureServicesActions.Add(services =>
            {
                services.TryAddSingleton(cliMode);
                services.AddSingleton(typeof(ICliMode), (IServiceProvider sp) => sp.GetRequiredService(cliMode));
            });

            if (asStartup)
            {
                _startupMode = _startupMode is null ? cliMode : throw new ArgumentException($"Only one mode can be registered as startup mode.", nameof(asStartup));
            }

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

            _middlewareTypes.AddLast(middleware);

            return this;
        }

        /// <summary>
        /// Adds a middleware to the command execution pipeline.
        /// </summary>
        public CliApplicationBuilder UseMiddleware<TMiddleware>()
            where TMiddleware : class, IMiddleware
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
            return UseHelpWriter(typeof(T));
        }
        #endregion

        #region Configuration
        //TODO: add configuration builder on actions https://github.com/aspnet/Hosting/blob/f9d145887773e0c650e66165e0c61886153bcc0b/src/Microsoft.Extensions.Hosting/HostBuilder.cs

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
        /// Configures application services.
        /// </summary>
        public CliApplicationBuilder ConfigureLogging(Action<ILoggingBuilder> action)
        {
            _configureServicesActions.Add(services =>
            {
                services.AddLogging(action);
            });

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
            where TContainerBuilder : notnull
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

        /// <summary>
        /// Creates an instance of <see cref="CliApplication"/> using configured parameters.
        /// Default values are used in place of parameters that were not specified.
        /// A scope is defined as a lifetime of a command execution pipeline that includes directives handling.
        /// </summary>
        public CliApplication Build()
        {
            if (_cliApplicationBuilt)
            {
                throw new InvalidOperationException("Build can only be called once.");
            }

            _cliApplicationBuilt = true;

            // Set default values
            _title ??= AssemblyUtils.TryGetDefaultTitle() ?? "App";
            _executableName ??= AssemblyUtils.TryGetDefaultExecutableName() ?? "app";
            _versionText ??= AssemblyUtils.TryGetDefaultVersionText() ?? "v1.0";

            if (_console is null)
            {
                UseConsole<SystemConsole>();
            }

            if (_startupMode is null || _modeTypes.Count == 0)
            {
                this.UseDirectMode(true);
            }

            // Add core middlewares to the end of the pipeline
            this.AddAfterUserMiddlewares();

            // Create context
            ServiceCollection _serviceCollection = new();

            ApplicationMetadata metadata = new(_title, _executableName, _versionText, _description);
            ApplicationConfiguration configuration = new(_modeTypes,
                                                         _commandTypes,
                                                         _directivesTypes,
                                                         _middlewareTypes,
                                                         _startupMode!,
                                                         _serviceCollection);

            EnvironmentVariablesAccessor environmentVariablesAccessor = new();

            // Add core services
            _serviceCollection.AddOptions();
            _serviceCollection.AddSingleton(typeof(ApplicationMetadata), metadata);
            _serviceCollection.AddSingleton(typeof(ApplicationConfiguration), configuration);
            _serviceCollection.AddSingleton(typeof(IConsole), _console ?? throw new NullReferenceException("'_console' must not be null."));
            _serviceCollection.AddSingleton(typeof(IEnvironmentVariablesAccessor), environmentVariablesAccessor);
            _serviceCollection.AddSingleton<IRootSchemaAccessor, RootSchemaAccessor>();
            _serviceCollection.AddSingleton<ICliCommandExecutor, CliCommandExecutor>();
            _serviceCollection.AddSingleton<ICliApplicationLifetime, CliApplicationLifetime>();

            _serviceCollection.AddScoped(typeof(ICliContext), (provider) =>
            {
                IRootSchemaAccessor rootSchemaAccessor = provider.GetRequiredService<IRootSchemaAccessor>();

                return new CliContext(metadata,
                                      configuration,
                                      rootSchemaAccessor.RootSchema,
                                      environmentVariablesAccessor.EnvironmentVariables,
                                      _console);
            });

            _serviceCollection.AddLogging(cfg =>
            {
                cfg.ClearProviders();
                cfg.AddDebug();
                cfg.SetMinimumLevel(LogLevel.Information);
            });

            IServiceProvider serviceProvider = CreateServiceProvider(_serviceCollection);

            return new CliApplication(serviceProvider,
                                      _console,
                                      _isSelfCreatedConsoleInstance,
                                      environmentVariablesAccessor,
                                      metadata,
                                      _startupMessage);
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