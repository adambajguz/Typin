namespace Typin
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text.RegularExpressions;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;
    using Typin.AutoCompletion;
    using Typin.Console;
    using Typin.Directives;
    using Typin.Exceptions;
    using Typin.Internal.DependencyInjection;
    using Typin.Internal.Extensions;
    using Typin.Internal.Pipeline;
    using Typin.OptionFallback;
    using Typin.Schemas;

    /// <summary>
    /// Builds an instance of <see cref="CliApplication"/>.
    /// </summary>
    public sealed partial class CliApplicationBuilder
    {
        private bool _cliApplicationBuilt;

        //Directives and commands settings
        private readonly List<Type> _commandTypes = new List<Type>();
        private readonly List<Type> _customDirectives = new List<Type>();

        //Metadata settings
        private string? _title;
        private string? _executableName;
        private string? _versionText;
        private string? _description;
        private string? _startupMessage;

        //Exceptions
        private ICliExceptionHandler? _exceptionHandler;

        //Console
        private IConsole? _console;

        //Dependency injection
        private IServiceFactoryAdapter _serviceProviderFactory = new ServiceFactoryAdapter<IServiceCollection>(new DefaultServiceProviderFactory());
        private readonly List<Action<IServiceCollection>> _configureServicesActions = new List<Action<IServiceCollection>>();
        private readonly List<IConfigureContainerAdapter> _configureContainerActions = new List<IConfigureContainerAdapter>();

        //Interactive mode settings
        private bool _useInteractiveMode = false;
        private bool _useAdvancedInput = false;
        private ConsoleColor _promptForeground = ConsoleColor.Blue;
        private ConsoleColor _commandForeground = ConsoleColor.Yellow;
        private HashSet<ShortcutDefinition>? _userDefinedShortcuts;

        //Middleware
        private readonly LinkedList<Type> _middlewareTypes = new LinkedList<Type>();

        // Value fallback

        #region Directives
        /// <summary>
        /// Add a custom directive to the application.
        /// </summary>
        public CliApplicationBuilder AddDirective(Type directiveType)
        {
            _customDirectives.Add(directiveType);

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
            foreach (var directiveType in directiveTypes)
                AddDirective(directiveType);

            return this;
        }

        /// <summary>
        /// Adds directives from the specified assembly to the application.
        /// Only adds public valid directive types.
        /// </summary>
        public CliApplicationBuilder AddDirectivesFrom(Assembly directiveAssembly)
        {
            foreach (var directiveType in directiveAssembly.ExportedTypes.Where(CommandSchema.IsCommandType))
                AddCommand(directiveType);

            return this;
        }

        /// <summary>
        /// Adds directives from the specified assemblies to the application.
        /// Only adds public valid directive types.
        /// </summary>
        public CliApplicationBuilder AddDirectivesFrom(IEnumerable<Assembly> directiveAssemblies)
        {
            foreach (var directiveType in directiveAssemblies)
                AddCommandsFrom(directiveType);

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
            foreach (var commandType in commandTypes)
                AddCommand(commandType);

            return this;
        }

        /// <summary>
        /// Adds commands from the specified assembly to the application.
        /// Only adds public valid command types.
        /// </summary>
        public CliApplicationBuilder AddCommandsFrom(Assembly commandAssembly)
        {
            foreach (var commandType in commandAssembly.ExportedTypes.Where(CommandSchema.IsCommandType))
                AddCommand(commandType);

            return this;
        }

        /// <summary>
        /// Adds commands from the specified assemblies to the application.
        /// Only adds public valid command types.
        /// </summary>
        public CliApplicationBuilder AddCommandsFrom(IEnumerable<Assembly> commandAssemblies)
        {
            foreach (var commandAssembly in commandAssemblies)
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

            return this;
        }

        /// <summary>
        /// Configures the application to use the specified implementation of <see cref="IConsole"/>.
        /// </summary>
        public CliApplicationBuilder UseConsole<T>()
            where T : class, IConsole, new()
        {
            _console = new T();

            return this;
        }
        #endregion

        #region Exceptions
        /// <summary>
        /// Configures the application to use the specified implementation of <see cref="ICliExceptionHandler"/>.
        /// </summary>
        public CliApplicationBuilder UseExceptionHandler<T>()
            where T : class, ICliExceptionHandler, new()
        {
            _exceptionHandler = new T();

            return this;
        }

        /// <summary>
        /// Configures the application to use the specified implementation of <see cref="ICliExceptionHandler"/>.
        /// </summary>
        public CliApplicationBuilder UseExceptionHandler(ICliExceptionHandler handler)
        {
            _exceptionHandler = handler;

            return this;
        }
        #endregion

        #region Interactive Mode
        /// <summary>
        /// Configures whether interactive mode (enabled with [interactive] directive) is allowed in the application.
        /// By default this adds [default], [>], [.], and [..] and advanced command input.
        ///
        /// If you wish to add only [default] directive, set addScopeDirectives to false.
        /// If you wish to disable history and auto completion set useAdvancedInput to false.
        /// </summary>
        public CliApplicationBuilder UseInteractiveMode(bool addScopeDirectives = true,
                                                        bool useAdvancedInput = true,
                                                        HashSet<ShortcutDefinition>? userDefinedShortcuts = null)
        {
            _useInteractiveMode = true;
            _useAdvancedInput = useAdvancedInput;

            AddDirective<DefaultDirective>();

            if (addScopeDirectives)
            {
                AddDirective<ScopeDirective>();
                AddDirective<ScopeResetDirective>();
                AddDirective<ScopeUpDirective>();
            }

            _userDefinedShortcuts = userDefinedShortcuts;

            return this;
        }

        /// <summary>
        /// Configures the command prompt foreground color in interactive mode.
        /// </summary>
        public CliApplicationBuilder UsePromptForeground(ConsoleColor color)
        {
            _promptForeground = color;

            return this;
        }

        /// <summary>
        /// Configures the command input foreground color in interactive mode.
        /// </summary>
        public CliApplicationBuilder UseCommandInputForeground(ConsoleColor color)
        {
            _commandForeground = color;

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
            _serviceProviderFactory = new ServiceFactoryAdapter<TContainerBuilder>(factory ?? throw new ArgumentNullException(nameof(factory)));

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
        /// </summary>
        public CliApplicationBuilder UseMiddleware(Type middleware)
        {
            _configureServicesActions.Add(services =>
            {
                services.AddSingleton(typeof(IMiddleware), middleware);
                services.AddSingleton(middleware);
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
        /// Configures to use a specific option fallback provider with desired lifetime instead of <see cref="EnvironmentVariableFallbackProvider"/>.
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
        /// Configures to use a specific option fallback provider with desired lifetime instead of <see cref="EnvironmentVariableFallbackProvider"/>.
        /// </summary>
        public CliApplicationBuilder UseOptionFallbackProvider<T>(ServiceLifetime lifetime = ServiceLifetime.Singleton)
            where T : IOptionFallbackProvider
        {
            return UseOptionFallbackProvider(typeof(T), lifetime);
        }
        #endregion

        /// <summary>
        /// Creates an instance of <see cref="CliApplication"/> or <see cref="InteractiveCliApplication"/> using configured parameters.
        /// Default values are used in place of parameters that were not specified.
        /// </summary>
        public CliApplication Build()
        {
            if (_cliApplicationBuilt)
                throw new InvalidOperationException("Build can only be called once.");

            _cliApplicationBuilt = true;

            // Set default values
            _title ??= TryGetDefaultTitle() ?? "App";
            _executableName ??= TryGetDefaultExecutableName() ?? "app";
            _versionText ??= TryGetDefaultVersionText() ?? "v1.0";
            _console ??= new SystemConsole();
            _exceptionHandler ??= new DefaultExceptionHandler();
            _userDefinedShortcuts ??= new HashSet<ShortcutDefinition>();

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

            // Add core middlewares
            UseMiddleware<CommandExecution>();

            // Create context
            var metadata = new ApplicationMetadata(_title, _executableName, _versionText, _description, _startupMessage);
            var configuration = new ApplicationConfiguration(_commandTypes,
                                                             _customDirectives,
                                                             _exceptionHandler,
                                                             _useInteractiveMode,
                                                             _useAdvancedInput);

            var _serviceCollection = new ServiceCollection();
            var cliContext = new CliContext(metadata, configuration, _serviceCollection, _console, _middlewareTypes);

            // Add core services
            _serviceCollection.AddSingleton(typeof(ApplicationMetadata), (provider) => metadata);
            _serviceCollection.AddSingleton(typeof(ApplicationConfiguration), (provider) => configuration);
            _serviceCollection.AddSingleton(typeof(IConsole), (provider) => _console);
            _serviceCollection.AddSingleton(typeof(ICliContext), (provider) => cliContext);

            IServiceProvider serviceProvider = CreateServiceProvider(_serviceCollection);

            // Create application instance
            if (_useInteractiveMode)
            {
                return new InteractiveCliApplication(serviceProvider,
                                                     cliContext,
                                                     _promptForeground,
                                                     _commandForeground,
                                                     _userDefinedShortcuts);
            }

            return new CliApplication(serviceProvider, cliContext);
        }

        private IServiceProvider CreateServiceProvider(ServiceCollection services)
        {
            foreach (Action<IServiceCollection> configureServicesAction in _configureServicesActions)
            {
                configureServicesAction(services);
            }
            services.TryAddSingleton<IOptionFallbackProvider, EnvironmentVariableFallbackProvider>();

            object? containerBuilder = _serviceProviderFactory.CreateBuilder(services);
            foreach (IConfigureContainerAdapter containerAction in _configureContainerActions)
            {
                containerAction.ConfigureContainer(containerBuilder);
            }

            IServiceProvider? appServices = _serviceProviderFactory.CreateServiceProvider(containerBuilder);

            return appServices ?? throw new InvalidOperationException($"The IServiceProviderFactory returned a null IServiceProvider.");
        }
    }

    public partial class CliApplicationBuilder
    {
        private static readonly Lazy<Assembly?> LazyEntryAssembly = new Lazy<Assembly?>(Assembly.GetEntryAssembly);

        // Entry assembly is null in tests
        private static Assembly? EntryAssembly => LazyEntryAssembly.Value;

        private static string? TryGetDefaultTitle()
        {
            return EntryAssembly?.GetName().Name;
        }

        private static string? TryGetDefaultExecutableName()
        {
            string? entryAssemblyLocation = EntryAssembly?.Location;

            // The assembly can be an executable or a dll, depending on how it was packaged
            bool isDll = string.Equals(Path.GetExtension(entryAssemblyLocation), ".dll", StringComparison.OrdinalIgnoreCase);

            return isDll
                ? "dotnet " + Path.GetFileName(entryAssemblyLocation)
                : Path.GetFileNameWithoutExtension(entryAssemblyLocation);
        }

        private static string? TryGetDefaultVersionText()
        {
            return EntryAssembly != null ? $"v{EntryAssembly.GetName().Version.ToSemanticString()}" : null;
        }
    }
}