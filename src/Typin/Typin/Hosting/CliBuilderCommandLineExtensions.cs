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
        /// <param name="startsWithExecutableName">Whether <paramref name="commandLine"/> starts with executable name that should be ommited (default: false).</param>
        /// <returns></returns>
        public static ICliBuilder OverrideCommandLine(this ICliBuilder builder, string commandLine, bool startsWithExecutableName = false)
        {
            builder.Services.Configure<CliOptions>(options =>
            {
                options.CommandLine = commandLine;
                options.CommandLineStartsWithExecutableName = startsWithExecutableName;
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
            return builder.OverrideCommandLine(false, args);
        }

        /// <summary>
        /// Sets a custom command line.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="args">Command arguments override.</param>
        /// <param name="containsExecutable">Whether <paramref name="args"/> starts with executable name that should be ommited (default: false).</param>
        /// <returns></returns>
        public static ICliBuilder OverrideCommandLine(this ICliBuilder builder, bool containsExecutable, params string[] args)
        {
            string commandLine = '"' + string.Join(@""" """, args) + '"';

            return builder.OverrideCommandLine(commandLine, containsExecutable);
        }

        /// <summary>
        /// Sets a custom command line.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="args">Command arguments override.</param>
        /// <param name="containsExecutable">Whether <paramref name="args"/> starts with executable name that should be ommited (default: false).</param>
        /// <returns></returns>
        public static ICliBuilder OverrideCommandLine(this ICliBuilder builder, IEnumerable<string> args, bool containsExecutable = false)
        {
            string commandLine = '"' + string.Join(@""" """, args) + '"';

            return builder.OverrideCommandLine(commandLine, containsExecutable);
        }
    }
}
