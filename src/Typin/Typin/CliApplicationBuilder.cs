namespace Typin
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Typin.Commands;
    using Typin.Console;
    using Typin.Directives;
    using Typin.Help;
    using Typin.Hosting;
    using Typin.Modes;

    /// <summary>
    /// Builds an instance of <see cref="CliApplication"/>.
    /// </summary>
    public sealed class CliApplicationBuilder
    {
        private readonly List<Action<IHostBuilder>> _hostBuilderActions = new();
        private readonly List<Action<ICliBuilder>> _cliBuilderActions = new();

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
            _cliBuilderActions.Add((cliBuilder) =>
            {
                //cliBuilder.AddDirectives(directives =>
                //{
                //    directives.Single(directiveType);
                //});
            });

            return this;
        }

        /// <summary>
        /// Add a custom directive to the application.
        /// </summary>
        public CliApplicationBuilder AddDirective<T>()
            where T : class, IDirective
        {
            return AddDirective(typeof(T));
        }

        /// <summary>
        /// Add custom directives to the application.
        /// </summary>
        public CliApplicationBuilder AddDirectives(IEnumerable<Type> directiveTypes)
        {
            _cliBuilderActions.Add((cliBuilder) =>
            {
                //cliBuilder.AddDirectives(directives =>
                //{
                //    directives.Multiple(directiveTypes);
                //});
            });

            return this;
        }

        /// <summary>
        /// Adds directives from the specified assembly to the application.
        /// Only adds public valid directive types.
        /// </summary>
        public CliApplicationBuilder AddDirectivesFrom(Assembly directiveAssembly)
        {
            _cliBuilderActions.Add((cliBuilder) =>
            {
                //cliBuilder.AddDirectives(directives =>
                //{
                //    directives.From(directiveAssembly);
                //});
            });

            return this;
        }

        /// <summary>
        /// Adds directives from the specified assemblies to the application.
        /// Only adds public valid directive types.
        /// </summary>
        public CliApplicationBuilder AddDirectivesFrom(IEnumerable<Assembly> directiveAssemblies)
        {
            _cliBuilderActions.Add((cliBuilder) =>
            {
                //cliBuilder.AddDirectives(directives =>
                //{
                //    directives.From(directiveAssemblies);
                //});
            });

            return this;
        }

        /// <summary>
        /// Adds directives from the calling assembly to the application.
        /// Only adds public valid directive types.
        /// </summary>
        public CliApplicationBuilder AddDirectivesFromThisAssembly()
        {
            _cliBuilderActions.Add((cliBuilder) =>
            {
                //cliBuilder.AddDirectives(directives =>
                //{
                //    directives.FromThisAssembly();
                //});
            });

            return this;
        }
        #endregion

        #region Commands
        /// <summary>
        /// Adds a command of specified type to the application.
        /// </summary>
        public CliApplicationBuilder AddCommand(Type commandType)
        {
            _cliBuilderActions.Add((cliBuilder) =>
            {
                cliBuilder.AddCommands(commands =>
                {
                    commands.Single(commandType);
                });
            });

            return this;
        }

        /// <summary>
        /// Adds a command of specified type to the application.
        /// </summary>
        public CliApplicationBuilder AddCommand<T>()
            where T : class, ICommand
        {
            return AddCommand(typeof(T));
        }

        /// <summary>
        /// Adds multiple commands to the application.
        /// </summary>
        public CliApplicationBuilder AddCommands(IEnumerable<Type> commandTypes)
        {
            _cliBuilderActions.Add((cliBuilder) =>
            {
                cliBuilder.AddCommands(commands =>
                {
                    commands.Multiple(commandTypes);
                });
            });

            return this;
        }

        /// <summary>
        /// Adds commands from the specified assembly to the application.
        /// Only adds public valid command types.
        /// </summary>
        public CliApplicationBuilder AddCommandsFrom(Assembly commandAssembly)
        {
            _cliBuilderActions.Add((cliBuilder) =>
            {
                cliBuilder.AddCommands(commands =>
                {
                    commands.From(commandAssembly);
                });
            });

            return this;
        }

        /// <summary>
        /// Adds commands from the specified assemblies to the application.
        /// Only adds public valid command types.
        /// </summary>
        public CliApplicationBuilder AddCommandsFrom(IEnumerable<Assembly> commandAssemblies)
        {
            _cliBuilderActions.Add((cliBuilder) =>
            {
                cliBuilder.AddCommands(commands =>
                {
                    commands.From(commandAssemblies);
                });
            });

            return this;
        }

        /// <summary>
        /// Adds commands from the calling assembly to the application.
        /// Only adds public valid command types.
        /// </summary>
        public CliApplicationBuilder AddCommandsFromThisAssembly()
        {
            _cliBuilderActions.Add((cliBuilder) =>
            {
                cliBuilder.AddCommands(commands =>
                {
                    commands.FromThisAssembly();
                });
            });

            return this;
        }
        #endregion

        #region Metadata
        /// <summary>
        /// Sets application title, which appears in the help text.
        /// </summary>
        public CliApplicationBuilder UseTitle(string title)
        {
            _cliBuilderActions.Add((cliBuilder) =>
            {
                cliBuilder.UseTitle(title);
            });

            return this;
        }

        /// <summary>
        /// Sets application executable name, which appears in the help text.
        /// </summary>
        public CliApplicationBuilder UseExecutableName(string executableName)
        {
            _cliBuilderActions.Add((cliBuilder) =>
            {
                cliBuilder.UseExecutableName(executableName);
            });

            return this;
        }

        /// <summary>
        /// Sets application version text, which appears in the help text and when the user requests version information.
        /// </summary>
        public CliApplicationBuilder UseVersionText(string versionText)
        {
            _cliBuilderActions.Add((cliBuilder) =>
            {
                cliBuilder.UseVersionText(versionText);
            });

            return this;
        }

        /// <summary>
        /// Sets application description, which appears in the help text.
        /// </summary>
        public CliApplicationBuilder UseDescription(string? description)
        {
            _cliBuilderActions.Add((cliBuilder) =>
            {
                cliBuilder.UseDescription(description);
            });

            return this;
        }
        #endregion

        #region Startup message
        /// <summary>
        /// Sets application startup message, which appears just after starting the app.
        /// </summary>
        public CliApplicationBuilder UseStartupMessage(string message, ConsoleColor messageColor = ConsoleColor.DarkYellow)
        {
            _cliBuilderActions.Add((cliBuilder) =>
            {
                cliBuilder.UseStartupMessage(message, messageColor);
            });

            return this;
        }

        /// <summary>
        /// Sets application startup message, which appears just after starting the app.
        /// </summary>
        public CliApplicationBuilder UseStartupMessage(Func<ApplicationMetadata, string> message, ConsoleColor messageColor = ConsoleColor.DarkYellow)
        {
            _cliBuilderActions.Add((cliBuilder) =>
            {
                cliBuilder.UseStartupMessage(message, messageColor);
            });

            return this;
        }

        /// <summary>
        /// Sets application startup message, which appears just after starting the app.
        /// </summary>
        public CliApplicationBuilder UseStartupMessage(Action<ApplicationMetadata, IConsole> message)
        {
            _cliBuilderActions.Add((cliBuilder) =>
            {
                cliBuilder.UseStartupMessage(message);
            });

            return this;
        }
        #endregion

        #region Console
        /// <summary>
        /// Configures the application to use the specified implementation of <see cref="IConsole"/>.
        /// </summary>
        public CliApplicationBuilder UseConsole(IConsole console)
        {
            _cliBuilderActions.Add((cliBuilder) =>
            {
                cliBuilder.UseConsole(console);
            });

            return this;
        }

        /// <summary>
        /// Configures the application to use the specified implementation of <see cref="IConsole"/>.
        /// </summary>
        public CliApplicationBuilder UseConsole<T>()
            where T : class, IConsole, new()
        {
            _cliBuilderActions.Add((cliBuilder) =>
            {
                cliBuilder.UseConsole<T>();
            });

            return this;
        }
        #endregion

        #region Modes
        /// <summary>
        /// Registers a CLI mode. Only one mode can be registered as startup mode.
        /// </summary>
        public CliApplicationBuilder RegisterMode<T>(bool asStartup = false)
            where T : class, ICliMode
        {
            return RegisterMode(typeof(T), asStartup);
        }

        /// <summary>
        /// Registers a CLI mode. Only one mode can be registered as startup mode.
        /// </summary>
        public CliApplicationBuilder RegisterMode(Type modeType, bool asStartup = false)
        {
            _cliBuilderActions.Add((cliBuilder) =>
            {
                cliBuilder.AddModes(modes =>
                {
                    modes.Single(modeType);
                });

                if (asStartup)
                {
                    cliBuilder.SetStartupMode(modeType);
                }
            });

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
            throw new NotImplementedException();
            //_configureServicesActions.Add(services =>
            //{
            //    services.AddScoped(typeof(IMiddleware), middleware);
            //    services.AddScoped(middleware);
            //});

            //_middlewareTypes.AddLast(middleware);

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
        /// Configures to use a specific option fallback provider with desired lifetime.
        /// </summary>
        public CliApplicationBuilder UseOptionFallbackProvider(Type fallbackProviderType, ServiceLifetime lifetime = ServiceLifetime.Singleton)
        {
            throw new NotImplementedException();

            return this;
        }

        /// <summary>
        /// Configures to use a specific option fallback provider with desired lifetime.
        /// </summary>
        public CliApplicationBuilder UseOptionFallbackProvider<T>(ServiceLifetime lifetime = ServiceLifetime.Singleton)
            where T : class//, IOptionFallbackProvider
        {
            return UseOptionFallbackProvider(typeof(T), lifetime);
        }
        #endregion

        #region Help Writer
        /// <summary>
        /// Configures to use a specific help writer with transient lifetime.
        /// </summary>
        public CliApplicationBuilder UseHelpWriter(Type helpWriterType, ServiceLifetime lifetime = ServiceLifetime.Scoped)
        {
            _cliBuilderActions.Add((cliBuilder) =>
            {
                cliBuilder.UseHelpWriter(helpWriterType);
            });

            return this;
        }

        /// <summary>
        /// Configures to use a specific help writer with transient lifetime.
        /// </summary>
        public CliApplicationBuilder UseHelpWriter<T>(ServiceLifetime lifetime = ServiceLifetime.Scoped)
            where T : class, IHelpWriter
        {
            _cliBuilderActions.Add((cliBuilder) =>
            {
                cliBuilder.UseHelpWriter<T>();
            });

            return this;
        }
        #endregion

        #region Configuration
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
            _hostBuilderActions.Add((hostBuilder) => hostBuilder.ConfigureServices(action));

            return this;
        }

        /// <summary>
        /// Configures application services.
        /// </summary>
        public CliApplicationBuilder ConfigureLogging(Action<ILoggingBuilder> action)
        {
            _hostBuilderActions.Add((hostBuilder) => hostBuilder.ConfigureLogging(action));

            return this;
        }

        //https://github.com/aspnet/Hosting/blob/f9d145887773e0c650e66165e0c61886153bcc0b/src/Microsoft.Extensions.Hosting/HostBuilder.cs

        /// <summary>
        /// Overrides the factory used to create the service provider.
        /// </summary>
        public CliApplicationBuilder UseServiceProviderFactory<TContainerBuilder>(IServiceProviderFactory<TContainerBuilder> factory)
            where TContainerBuilder : notnull
        {
            _hostBuilderActions.Add((hostBuilder) => hostBuilder.UseServiceProviderFactory(factory));

            return this;
        }

        /// <summary>
        /// Enables configuring the instantiated dependency container. This can be called multiple times and
        /// the results will be additive.
        /// </summary>
        public CliApplicationBuilder ConfigureContainer<TContainerBuilder>(Action<TContainerBuilder> configureDelegate)
        {
            _hostBuilderActions.Add((hostBuilder) => hostBuilder.ConfigureContainer(configureDelegate));

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
            IHostBuilder hostBuilder = CliHost.CreateDefaultBuilder()
                .ConfigureLogging((context, builder) =>
                {
                    builder.SetMinimumLevel(LogLevel.Warning);
                });

            foreach (Action<IHostBuilder> action in _hostBuilderActions)
            {
                action(hostBuilder);
            }

            RegisterMode<DirectMode>(asStartup: true);

            ExitCodeProvider exitCodeProvider = new();

            hostBuilder.ConfigureCliHost(cliBuilder =>
            {
                cliBuilder.CaptureExitCode(exitCodeProvider);

                foreach (Action<ICliBuilder> action in _cliBuilderActions)
                {
                    action(cliBuilder);
                }
            });

            return new CliApplication(hostBuilder, exitCodeProvider);
        }
    }
}