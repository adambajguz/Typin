namespace Typin.Hosting
{
    using System;
    using Microsoft.Extensions.DependencyInjection;
    using Typin.Console;

    /// <summary>
    /// <see cref="ICliBuilder"/> startup message extensions.
    /// </summary>
    public static class CliBuilderStartupMessagesExtensions
    {
        /// <summary>
        /// Clears applications startup message.
        /// </summary>
        public static ICliBuilder ClearStartupMessage(this ICliBuilder cliBuilder)
        {
            cliBuilder.Services.Configure<CliOptions>(options =>
            {
                options.StartupMessage = null;
            });

            return cliBuilder;
        }

        /// <summary>
        /// Sets application startup message, which appears just after starting the app.
        /// </summary>
        public static ICliBuilder UseStartupMessage(this ICliBuilder cliBuilder, string message, ConsoleColor messageColor = ConsoleColor.DarkYellow)
        {
            return cliBuilder.UseStartupMessage((serviceProvider, metadata, console) =>
            {
                console.Output.WithForegroundColor(messageColor, (output) => output.WriteLine(message));
            });
        }

        /// <summary>
        /// Sets application startup message, which appears just after starting the app.
        /// </summary>
        public static ICliBuilder UseStartupMessage(this ICliBuilder cliBuilder, Func<ApplicationMetadata, string> message, ConsoleColor messageColor = ConsoleColor.DarkYellow)
        {
            return cliBuilder.UseStartupMessage((serviceProvider, metadata, console) =>
            {
                string tmp = message(metadata);

                console.Output.WithForegroundColor(messageColor, (output) => output.WriteLine(tmp));
            });
        }

        /// <summary>
        /// Sets application startup message, which appears just after starting the app.
        /// </summary>
        public static ICliBuilder UseStartupMessage(this ICliBuilder cliBuilder, Action<ApplicationMetadata, IConsole>? message)
        {
            if (message is null)
            {
                return cliBuilder.ClearStartupMessage();
            }

            return cliBuilder.UseStartupMessage((serviceProvider, metadata, console) => message(metadata, console));
        }

        /// <summary>
        /// Sets application startup message, which appears just after starting the app.
        /// </summary>
        public static ICliBuilder UseStartupMessage(this ICliBuilder cliBuilder, Action<IServiceProvider, ApplicationMetadata, IConsole>? message)
        {
            cliBuilder.Services.Configure<CliOptions>(options =>
            {
                options.StartupMessage = message;
            });

            return cliBuilder;
        }
    }
}
