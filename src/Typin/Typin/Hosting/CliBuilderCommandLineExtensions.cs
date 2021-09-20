namespace Typin.Hosting
{
    using System.Collections.Generic;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// <see cref="ICliBuilder"/> command line extensions.
    /// </summary>
    public static class CliBuilderCommandLineExtensions
    {
        /// <summary>
        /// Sets a custom command line.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="commandLine">Command line override.</param>
        /// <param name="startupExecutionOptions">Startup command line execution options.</param>
        /// <returns></returns>
        public static ICliBuilder OverrideCommandLine(this ICliBuilder builder, string commandLine, CommandExecutionOptions startupExecutionOptions = default)
        {
            builder.Services.Configure<CliOptions>(options =>
            {
                options.CommandLine = commandLine;
                options.StartupExecutionOptions = startupExecutionOptions;
            });

            return builder;
        }

        /// <summary>
        /// Sets a custom command line that does not contain an executable name.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="args">Command arguments override.</param>
        /// <returns></returns>
        public static ICliBuilder OverrideCommandLine(this ICliBuilder builder, params string[] args)
        {
            return builder.OverrideCommandLine(CommandExecutionOptions.Default, args);
        }

        /// <summary>
        /// Sets a custom command line.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="args">Command arguments override.</param>
        /// <param name="startupExecutionOptions">Startup command line execution options.</param>
        /// <returns></returns>
        public static ICliBuilder OverrideCommandLine(this ICliBuilder builder, CommandExecutionOptions startupExecutionOptions, params string[] args)
        {
            string commandLine = '"' + string.Join(@""" """, args) + '"';

            return builder.OverrideCommandLine(commandLine, startupExecutionOptions);
        }

        /// <summary>
        /// Sets a custom command line.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="args">Command arguments override.</param>
        /// <param name="startupExecutionOptions">Startup command line execution options.</param>
        /// <returns></returns>
        public static ICliBuilder OverrideCommandLine(this ICliBuilder builder, IEnumerable<string> args, CommandExecutionOptions startupExecutionOptions = default)
        {
            string commandLine = '"' + string.Join(@""" """, args) + '"';

            return builder.OverrideCommandLine(commandLine, startupExecutionOptions);
        }
    }
}
