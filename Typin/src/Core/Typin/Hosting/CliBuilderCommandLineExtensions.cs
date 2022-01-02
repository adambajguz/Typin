namespace Typin.Hosting
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// <see cref="ICliBuilder"/> command line extensions.
    /// </summary>
    public static class CliBuilderCommandLineExtensions
    {
        /// <summary>
        /// Sets <see cref="CliOptions.CommandLine"/> (resets <see cref="CliOptions.CommandLineArguments"/> to null).
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="commandLine">Command line override.</param>
        /// <param name="startupInputOptions">Startup command line execution options.</param>
        /// <returns></returns>
        public static ICliBuilder OverrideCommandLine(this ICliBuilder builder, string commandLine, InputOptions startupInputOptions = default)
        {
            builder.Services.Configure<CliOptions>(options =>
            {
                options.CommandLine = commandLine;
                options.CommandLineArguments = null;
                options.StartupInputOptions = startupInputOptions;
            });

            return builder;
        }

        /// <summary>
        /// Sets <see cref="CliOptions.CommandLineArguments"/> (resets <see cref="CliOptions.CommandLine"/> to null) that does not contain an executable name.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="args">Command arguments override.</param>
        /// <returns></returns>
        public static ICliBuilder OverrideCommandLine(this ICliBuilder builder, params string[] args)
        {
            return builder.OverrideCommandLine(InputOptions.Default, args);
        }

        /// <summary>
        /// Sets <see cref="CliOptions.CommandLineArguments"/> (resets <see cref="CliOptions.CommandLine"/> to null).
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="args">Command arguments override.</param>
        /// <param name="startupInputOptions">Startup command line execution options.</param>
        /// <returns></returns>
        public static ICliBuilder OverrideCommandLine(this ICliBuilder builder, InputOptions startupInputOptions, params string[] args)
        {
            builder.Services.Configure<CliOptions>(options =>
            {
                options.CommandLine = null;
                options.CommandLineArguments = args;
                options.StartupInputOptions = startupInputOptions;
            });

            return builder;
        }

        /// <summary>
        /// Sets <see cref="CliOptions.CommandLineArguments"/> (resets <see cref="CliOptions.CommandLine"/> to null).
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="args">Command arguments override.</param>
        /// <param name="startupInputOptions">Startup command line execution options.</param>
        /// <returns></returns>
        public static ICliBuilder OverrideCommandLine(this ICliBuilder builder, IEnumerable<string> args, InputOptions startupInputOptions = default)
        {
            builder.Services.Configure<CliOptions>(options =>
            {
                options.CommandLine = null;
                options.CommandLineArguments = args.ToArray();
                options.StartupInputOptions = startupInputOptions;
            });

            return builder;
        }
    }
}
