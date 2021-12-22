﻿namespace Typin.Hosting
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
        /// <param name="startupExecutionOptions">Startup command line execution options.</param>
        /// <returns></returns>
        public static ICliBuilder OverrideCommandLine(this ICliBuilder builder, string commandLine, CommandExecutionOptions startupExecutionOptions = default)
        {
            builder.Services.Configure<CliOptions>(options =>
            {
                options.CommandLine = commandLine;
                options.CommandLineArguments = null;
                options.StartupExecutionOptions = startupExecutionOptions;
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
            return builder.OverrideCommandLine(CommandExecutionOptions.Default, args);
        }

        /// <summary>
        /// Sets <see cref="CliOptions.CommandLineArguments"/> (resets <see cref="CliOptions.CommandLine"/> to null).
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="args">Command arguments override.</param>
        /// <param name="startupExecutionOptions">Startup command line execution options.</param>
        /// <returns></returns>
        public static ICliBuilder OverrideCommandLine(this ICliBuilder builder, CommandExecutionOptions startupExecutionOptions, params string[] args)
        {
            builder.Services.Configure<CliOptions>(options =>
            {
                options.CommandLine = null;
                options.CommandLineArguments = args;
                options.StartupExecutionOptions = startupExecutionOptions;
            });

            return builder;
        }

        /// <summary>
        /// Sets <see cref="CliOptions.CommandLineArguments"/> (resets <see cref="CliOptions.CommandLine"/> to null).
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="args">Command arguments override.</param>
        /// <param name="startupExecutionOptions">Startup command line execution options.</param>
        /// <returns></returns>
        public static ICliBuilder OverrideCommandLine(this ICliBuilder builder, IEnumerable<string> args, CommandExecutionOptions startupExecutionOptions = default)
        {
            builder.Services.Configure<CliOptions>(options =>
            {
                options.CommandLine = null;
                options.CommandLineArguments = args.ToArray();
                options.StartupExecutionOptions = startupExecutionOptions;
            });

            return builder;
        }
    }
}