namespace Typin.Hosting
{
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// <see cref="ICliBuilder"/> exit code capture extensions.
    /// </summary>
    public static class CliBuilderExitCodeCaptureExtensions
    {
        /// <summary>
        /// Adds a singleton instance of <see cref="ExitCodeProvider"/>, where a host exit code will be stored before stopping the generic host.
        /// </summary>
        /// <param name="cliBuilder"></param>
        /// <param name="instance"></param>
        public static ICliBuilder CaptureExitCode(this ICliBuilder cliBuilder, ExitCodeProvider instance)
        {
            cliBuilder.Services.AddSingleton(instance);

            return cliBuilder;
        }
    }
}
