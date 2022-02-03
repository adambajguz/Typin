namespace Typin.Directives
{
    using System;
    using System.Linq;
    using Microsoft.Extensions.DependencyInjection;
    using Typin.Directives.Collections;
    using Typin.Directives.Resolvers;
    using Typin.Directives.Scanning;
    using Typin.Directives.Schemas;
    using Typin.Hosting;

    /// <summary>
    /// <see cref="ICliBuilder"/> directives-related extensions.
    /// </summary>
    public static class CliBuilderExtensions
    {
        #region AddDirectives
        /// <summary>
        /// Adds directives to CLI application.
        /// </summary>
        /// <param name="cliBuilder"></param>
        /// <returns></returns>
        public static IDirectiveScanner AddDirectives(this ICliBuilder cliBuilder)
        {
            return cliBuilder.GetScanner<IDirective, IDirectiveScanner>(
                (cli, current) =>
                {
                    return new AggregatedDirectiveScanner(cli, current);
                });
        }

        /// <summary>
        /// Adds directives to CLI application.
        /// </summary>
        /// <param name="cliBuilder"></param>
        /// <param name="scanner"></param>
        /// <returns></returns>
        public static ICliBuilder AddDirectives(this ICliBuilder cliBuilder, Action<IDirectiveScanner> scanner)
        {
            scanner(cliBuilder.AddDirectives());

            return cliBuilder;
        }
        #endregion

        #region ConfigureDirectives
        /// <summary>
        /// Adds directives to CLI application.
        /// </summary>
        /// <param name="cliBuilder"></param>
        /// <returns></returns>
        public static IConfigureDirectiveScanner ConfigureDirectives(this ICliBuilder cliBuilder)
        {
            return cliBuilder.GetScanner<IConfigureDirective, IConfigureDirectiveScanner>(
                (cli, current) =>
                {
                    IServiceCollection services = cli.Services;
                    if (!services.Any(x => x.ImplementationType == typeof(DirectiveSchemaProvider)))
                    {
                        services.AddSingleton<IDirectiveSchemaCollection, DirectiveSchemaCollection>();
                        services.AddScoped<IDirectiveSchemaProvider, DirectiveSchemaProvider>();
                        services.AddHostedService<DirectiveSchemaProviderHostedService>();
                        services.AddTransient(typeof(IDirectiveSchemaResolver<>), typeof(DirectiveSchemaResolver<>));
                    }

                    return new ConfigureDirectiveScanner(services, current);
                });
        }

        /// <summary>
        /// Adds directives to CLI application.
        /// </summary>
        /// <param name="cliBuilder"></param>
        /// <param name="scanner"></param>
        /// <returns></returns>
        public static ICliBuilder ConfigureDirectives(this ICliBuilder cliBuilder, Action<IConfigureDirectiveScanner> scanner)
        {
            scanner(cliBuilder.ConfigureDirectives());

            return cliBuilder;
        }
        #endregion
    }
}