namespace Typin.Commands
{
    using System;
    using System.Linq;
    using Microsoft.Extensions.DependencyInjection;
    using Typin.Commands.Collections;
    using Typin.Commands.Resolvers;
    using Typin.Commands.Scanning;
    using Typin.Hosting;

    /// <summary>
    /// <see cref="ICliBuilder"/> components extensions.
    /// </summary>
    public static class CliBuilderComponentsExtensions
    {
        #region AddCommands
        /// <summary>
        /// Adds commands to CLI application.
        /// </summary>
        /// <param name="cliBuilder"></param>
        /// <returns></returns>
        public static ICommandScanner AddCommands(this ICliBuilder cliBuilder)
        {
            return cliBuilder.GetScanner<ICommand, ICommandScanner>(
                (cli, current) =>
                {
                    return new AggregatedCommandScanner(cli, current);
                });
        }

        /// <summary>
        /// Adds commands to CLI application.
        /// </summary>
        /// <param name="cliBuilder"></param>
        /// <param name="scanner"></param>
        /// <returns></returns>
        public static ICliBuilder AddCommands(this ICliBuilder cliBuilder, Action<ICommandScanner> scanner)
        {
            scanner(cliBuilder.AddCommands());

            return cliBuilder;
        }
        #endregion

        #region ConfigureCommands
        /// <summary>
        /// Adds commands to CLI application.
        /// </summary>
        /// <param name="cliBuilder"></param>
        /// <returns></returns>
        public static IConfigureCommandScanner ConfigureCommands(this ICliBuilder cliBuilder)
        {
            return cliBuilder.GetScanner<IConfigureCommand, IConfigureCommandScanner>(
                (cli, current) =>
                {
                    IServiceCollection services = cli.Services;
                    if (!services.Any(x => x.ImplementationType == typeof(CommandSchemaProvider)))
                    {
                        services.AddSingleton<ICommandSchemaCollection, CommandSchemaCollection>();
                        services.AddScoped<ICommandSchemaProvider, CommandSchemaProvider>();
                        services.AddHostedService<CommandSchemaProviderHostedService>();
                        services.AddTransient(typeof(ICommandSchemaResolver<>), typeof(CommandSchemaResolver<>));
                    }

                    return new ConfigureCommandScanner(services, current);
                });
        }

        /// <summary>
        /// Adds commands to CLI application.
        /// </summary>
        /// <param name="cliBuilder"></param>
        /// <param name="scanner"></param>
        /// <returns></returns>
        public static ICliBuilder ConfigureCommands(this ICliBuilder cliBuilder, Action<IConfigureCommandScanner> scanner)
        {
            scanner(cliBuilder.ConfigureCommands());

            return cliBuilder;
        }
        #endregion

        #region AddDynamicCommands
        /// <summary>
        /// Adds dynamic commands to CLI application.
        /// </summary>
        /// <param name="cliBuilder"></param>
        /// <returns></returns>
        public static IDynamicCommandScanner AddDynamicCommands(this ICliBuilder cliBuilder)
        {
            return cliBuilder.GetScanner<IDynamicCommand, IDynamicCommandScanner>(
                (cli, current) =>
                {
                    //IServiceCollection services = cli.Services;
                    //if (!services.Any(x => x.ImplementationType == typeof(DynamicCommandSchemaProvider)))
                    //{
                    //    services.AddSingleton<IDynamicCommandSchemaCollection, DynamicCommandSchemaCollection>();
                    //    services.AddTransient<IDynamicCommandSchemaProvider, DynamicCommandSchemaProvider>();
                    //}

                    return new DynamicCommandScanner(cli.Services, current);
                });
        }

        /// <summary>
        /// Adds dynamic commands to CLI application.
        /// </summary>
        /// <param name="cliBuilder"></param>
        /// <param name="scanner"></param>
        /// <returns></returns>
        public static ICliBuilder AddDynamicCommands(this ICliBuilder cliBuilder, Action<IDynamicCommandScanner> scanner)
        {
            scanner(cliBuilder.AddDynamicCommands());

            return cliBuilder;
        }
        #endregion
    }
}