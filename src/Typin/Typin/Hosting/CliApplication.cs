namespace Typin.Hosting
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Command line application facade.
    /// </summary>
    public sealed class CliApplication
    {
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Initializes an instance of <see cref="CliApplication"/>.
        /// </summary>
        public CliApplication(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Runs the application with specified command line arguments and environment variables, and returns the exit code.
        /// </summary>
        /// <remarks>
        /// If a <see cref="CommandException"/>, <see cref="DirectiveException"/>, or <see cref="TypinException"/> is thrown during command execution,
        /// it will be handled and routed to the console. Additionally, if the debugger is not attached (i.e. the app is running in production),
        /// all other exceptions thrown within this method will be handled and routed to the console as well.
        /// </remarks>
        public async ValueTask<int> RunAsync(CancellationToken cancellationToken)
        {
            return 0;
        }
    }
}
