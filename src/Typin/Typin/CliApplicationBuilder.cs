//namespace Typin
//{
//    using System;
//    using System.Collections.Generic;
//    using System.Linq;
//    using System.Reflection;
//    using Microsoft.Extensions.DependencyInjection;
//    using Microsoft.Extensions.DependencyInjection.Extensions;
//    using Microsoft.Extensions.Logging;
//    using PackSite.Library.Pipelining;
//    using Typin.Console;
//    using Typin.DynamicCommands;
//    using Typin.Exceptions;
//    using Typin.Help;
//    using Typin.Internal;
//    using Typin.Internal.DependencyInjection;
//    using Typin.Internal.DynamicCommands;
//    using Typin.Internal.Pipeline;
//    using Typin.Modes;
//    using Typin.OptionFallback;
//    using Typin.Schemas;

//    /// <summary>
//    /// Builds an instance of <see cref="CliApplication"/>.
//    /// </summary>
//    [Obsolete]
//    public sealed class CliApplicationBuilder
//    {
//        private bool _cliApplicationBuilt;

//        //Directives and commands settings
//        private readonly HashSet<Type> _commandTypes = new();
//        private readonly HashSet<Type> _dynamicCommandTypes = new();
//        private readonly HashSet<Type> _directivesTypes = new();

//        //Metadata settings
//        private string? _title;
//        private string? _executableName;
//        private string? _versionText;
//        private string? _description;
//        private Action<ApplicationMetadata, IConsole>? _startupMessage;

//        //Console
//        private IConsole? _console;

//        //Modes
//        private readonly List<Type> _modeTypes = new();
//        private Type? _startupMode;

//        /// <summary>
//        /// Initializes an instance of <see cref="CliApplicationBuilder"/>.
//        /// </summary>
//        public CliApplicationBuilder()
//        {

//        }

//        #region Metadata
//        /// <summary>
//        /// Sets application title, which appears in the help text.
//        /// </summary>
//        public CliApplicationBuilder UseTitle(string title)
//        {
//            _title = title;

//            return this;
//        }

//        /// <summary>
//        /// Sets application executable name, which appears in the help text.
//        /// </summary>
//        public CliApplicationBuilder UseExecutableName(string executableName)
//        {
//            _executableName = executableName;

//            return this;
//        }

//        /// <summary>
//        /// Sets application version text, which appears in the help text and when the user requests version information.
//        /// </summary>
//        public CliApplicationBuilder UseVersionText(string versionText)
//        {
//            _versionText = versionText;

//            return this;
//        }

//        /// <summary>
//        /// Sets application description, which appears in the help text.
//        /// </summary>
//        public CliApplicationBuilder UseDescription(string? description)
//        {
//            _description = description;

//            return this;
//        }
//        #endregion

//        #region Startup message
//        /// <summary>
//        /// Sets application startup message, which appears just after starting the app.
//        /// </summary>
//        public CliApplicationBuilder UseStartupMessage(string message, ConsoleColor messageColor = ConsoleColor.DarkYellow)
//        {
//            _startupMessage = (metadata, console) =>
//            {
//                console.Output.WithForegroundColor(messageColor, (output) => output.WriteLine(message));
//            };

//            return this;
//        }

//        /// <summary>
//        /// Sets application startup message, which appears just after starting the app.
//        /// </summary>
//        public CliApplicationBuilder UseStartupMessage(Func<ApplicationMetadata, string> message, ConsoleColor messageColor = ConsoleColor.DarkYellow)
//        {
//            _startupMessage = (metadata, console) =>
//            {
//                string tmp = message(metadata);

//                console.Output.WithForegroundColor(messageColor, (output) => output.WriteLine(tmp));
//            };

//            return this;
//        }

//        /// <summary>
//        /// Sets application startup message, which appears just after starting the app.
//        /// </summary>
//        public CliApplicationBuilder UseStartupMessage(Action<ApplicationMetadata, IConsole> message)
//        {
//            _startupMessage = message;

//            return this;
//        }
//        #endregion

//        #region Console
//        /// <summary>
//        /// Configures the application to use the specified implementation of <see cref="IConsole"/>.
//        /// Console will be automatically diposed before exiting from <see cref="CliApplication.RunAsync(IEnumerable{string}, IReadOnlyDictionary{string, string})"/>.
//        /// </summary>
//        public CliApplicationBuilder UseConsole(IConsole console)
//        {
//            _console = console;

//            _configureServicesActions.Add(services =>
//            {
//                services.AddSingleton<IConsole>(_console);
//            });

//            return this;
//        }

//        /// <summary>
//        /// Configures the application to use the specified implementation of <see cref="IConsole"/>.
//        /// </summary>
//        public CliApplicationBuilder UseConsole<T>()
//            where T : class, IConsole, new()
//        {
//            return UseConsole(new T());
//        }
//        #endregion

//        #region Exceptions
//        /// <summary>
//        /// Configures the application to use the specified implementation of <see cref="ICliExceptionHandler"/>.
//        /// Exception handler is configured as scoped service.
//        /// </summary>
//        public CliApplicationBuilder UseExceptionHandler(Type exceptionHandlerType)
//        {
//            _configureServicesActions.Add(services =>
//            {
//                services.AddScoped(typeof(ICliExceptionHandler), exceptionHandlerType);
//            });

//            return this;
//        }

//        /// <summary>
//        /// Configures the application to use the specified implementation of <see cref="ICliExceptionHandler"/>.
//        /// Exception handler is configured as scoped service.
//        /// </summary>
//        public CliApplicationBuilder UseExceptionHandler<T>()
//            where T : class, ICliExceptionHandler
//        {
//            return UseExceptionHandler(typeof(T));
//        }
//        #endregion

//        #region Modes
//        /// <summary>
//        /// Registers a CLI mode. Only one mode can be registered as startup mode.
//        /// If no mode was registered or none of the registered modes was marked as startup, <see cref="DirectMode"/> will be registered.
//        ///
//        /// Do not call RegisterMode directly from builder, instead call UseXMode method, e.g. UseDirectMode().
//        /// </summary>
//        public CliApplicationBuilder RegisterMode<T>(bool asStartup = false)
//            where T : ICliMode
//        {
//            return RegisterMode(typeof(T), asStartup);
//        }

//        /// <summary>
//        /// Registers a CLI mode. Only one mode can be registered as startup mode.
//        /// If no mode was registered or none of the registered modes was marked as startup, <see cref="DirectMode"/> will be registered.
//        ///
//        /// Do not call RegisterMode directly from builder, instead call UseXMode method, e.g. UseDirectMode().
//        /// </summary>
//        public CliApplicationBuilder RegisterMode(Type modeType, bool asStartup = false)
//        {
//            Type cliMode = modeType;
//            _modeTypes.Add(cliMode);

//            if (!KnownTypesHelpers.IsCliModeType(modeType))
//            {
//                throw new ArgumentException($"Invalid CLI mode type '{modeType}'.", nameof(modeType));
//            }

//            _configureServicesActions.Add(services =>
//            {
//                services.TryAddSingleton(cliMode);
//                services.AddSingleton(typeof(ICliMode), (IServiceProvider sp) => sp.GetRequiredService(cliMode));
//            });

//            if (asStartup)
//            {
//                _startupMode = _startupMode is null ? cliMode : throw new ArgumentException($"Only one mode can be registered as startup mode.", nameof(asStartup));
//            }

//            return this;
//        }
//        #endregion

//        #region Help Writer
//        /// <summary>
//        /// Configures to use a specific help writer with transient lifetime.
//        /// </summary>
//        public CliApplicationBuilder UseHelpWriter(Type helpWriterType)
//        {
//            _configureServicesActions.Add(services =>
//            {
//                services.AddTransient(typeof(IHelpWriter), helpWriterType);
//            });

//            return this;
//        }

//        /// <summary>
//        /// Configures to use a specific help writer with transient lifetime.
//        /// </summary>
//        public CliApplicationBuilder UseHelpWriter<T>()
//            where T : IHelpWriter
//        {
//            return UseHelpWriter(typeof(T));
//        }
//        #endregion

//        /// <summary>
//        /// Creates an instance of <see cref="CliApplication"/> using configured parameters.
//        /// Default values are used in place of parameters that were not specified.
//        /// A scope is defined as a lifetime of a command execution pipeline that includes directives handling.
//        /// </summary>
//        public CliApplication Build()
//        {
//            if (_cliApplicationBuilt)
//            {
//                throw new InvalidOperationException("Build can only be called once.");
//            }

//            _cliApplicationBuilt = true;

//            // Set default values
//            _title ??= AssemblyUtils.TryGetDefaultTitle() ?? "App";
//            _executableName ??= AssemblyUtils.TryGetDefaultExecutableName() ?? "app";
//            _versionText ??= AssemblyUtils.TryGetDefaultVersionText() ?? "v1.0";
//            _console ??= new SystemConsole();

//            if (_startupMode is null || _modeTypes.Count == 0)
//            {
//                this.UseDirectMode(true);
//            }

//            // Create context
//            ServiceCollection _serviceCollection = new();

//            ApplicationMetadata metadata = new(_title, _executableName, _versionText, _description);
//            ApplicationConfiguration configuration = new(_modeTypes,
//                                                         _commandTypes,
//                                                         _dynamicCommandTypes,
//                                                         _directivesTypes,
//                                                         _startupMode!,
//                                                         _serviceCollection);

//            EnvironmentVariablesAccessor environmentVariablesAccessor = new();

//            // Add core services
//            _serviceCollection.AddOptions();
//            _serviceCollection.AddPipelining();
//            _serviceCollection.AddSingleton(typeof(ApplicationMetadata), metadata);
//            _serviceCollection.AddSingleton(typeof(ApplicationConfiguration), configuration);
//            _serviceCollection.AddSingleton(typeof(IConsole), _console);
//            _serviceCollection.AddSingleton(typeof(IEnvironmentVariablesAccessor), environmentVariablesAccessor);
//            _serviceCollection.AddSingleton<IRootSchemaAccessor, RootSchemaAccessor>();
//            _serviceCollection.AddSingleton<ICliCommandExecutor, CliCommandExecutor>();
//            _serviceCollection.AddSingleton<ICliApplicationLifetime, CliApplicationLifetime>();

//            if (_dynamicCommandTypes.Count > 0)
//            {
//                _serviceCollection.AddSingleton<IDynamicCommandBuilderFactory, DynamicCommandBuilderFactory>();
//            }

//            _serviceCollection.AddScoped(typeof(ICliContext), (provider) =>
//            {
//                IRootSchemaAccessor rootSchemaAccessor = provider.GetRequiredService<IRootSchemaAccessor>();

//                return new CliContext(metadata,
//                                      configuration,
//                                      rootSchemaAccessor.RootSchema,
//                                      environmentVariablesAccessor.EnvironmentVariables,
//                                      _console);
//            });

//            _serviceCollection.AddLogging(cfg =>
//            {
//                cfg.ClearProviders();
//                cfg.AddDebug();
//                cfg.SetMinimumLevel(LogLevel.Information);
//            });

//            IServiceProvider serviceProvider = CreateServiceProvider(_serviceCollection);
//            var pipelineCollection = serviceProvider.GetRequiredService<IPipelineCollection>();

//            var pipelineBuilder = PipelineBuilder.Create<ICliContext>()
//                .Lifetime(InvokablePipelineLifetime.Scoped)
//                .Add<ResolveCommandSchemaAndInstance>()
//                .Add<InitializeDirectives>()
//                .Add<ExecuteDirectivesSubpipeline>()
//                .Add<HandleSpecialOptions>()
//                .Add<BindInput>()
//                // user
//                .Add<ExecuteCommand>()
//                .Build().TryAddTo(pipelineCollection);

//            return new CliApplication(serviceProvider, _console, environmentVariablesAccessor, metadata, _startupMessage);
//        }

//        private IServiceProvider CreateServiceProvider(ServiceCollection services)
//        {
//            foreach (Action<IServiceCollection> configureServicesAction in _configureServicesActions)
//            {
//                configureServicesAction(services);
//            }

//            services.TryAddSingleton<IOptionFallbackProvider, EnvironmentVariableFallbackProvider>();
//            services.TryAddScoped<ICliExceptionHandler, DefaultExceptionHandler>();
//            services.TryAddScoped<IHelpWriter, DefaultHelpWriter>();

//            object? containerBuilder = _serviceProviderAdapter.CreateBuilder(services);
//            foreach (IConfigureContainerAdapter containerAction in _configureContainerActions)
//            {
//                containerAction.ConfigureContainer(containerBuilder);
//            }

//            IServiceProvider? appServices = _serviceProviderAdapter.CreateServiceProvider(containerBuilder);

//            return appServices ?? throw new InvalidOperationException($"{nameof(IServiceFactoryAdapter)} returned a null instance of object implementing {nameof(IServiceProvider)}.");
//        }
//    }
//}