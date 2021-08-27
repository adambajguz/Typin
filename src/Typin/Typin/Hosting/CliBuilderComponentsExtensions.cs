namespace Typin.Hosting
{
    using System;
    using Typin.Commands;
    using Typin.Components;
    using Typin.Directives;
    using Typin.Modes;

    /// <summary>
    /// <see cref="ICliBuilder"/> components extensions.
    /// </summary>
    public static class CliBuilderComponentsExtensions
    {
        /// <summary>
        /// Adds commands to CLI application.
        /// </summary>
        /// <param name="cliBuilder"></param>
        /// <param name="scanner"></param>
        /// <returns></returns>
        public static ICliBuilder AddCommands(this ICliBuilder cliBuilder, Action<IScanner<ICommand>> scanner)
        {
            return cliBuilder.GetOrAddScanner<ICommand>(
                services =>
                {
                    return new CommandScanner(services);
                }, scanner);
        }

        /// <summary>
        /// Adds commands to CLI application.
        /// </summary>
        /// <param name="cliBuilder"></param>
        /// <param name="scanner"></param>
        /// <returns></returns>
        public static ICliBuilder AddDynamicCommands(this ICliBuilder cliBuilder, Action<IScanner<IDynamicCommand>> scanner)
        {
            return cliBuilder.GetOrAddScanner<IDynamicCommand>(
                services =>
                {
                    return new DynamicCommandScanner(services);
                }, scanner);
        }

        /// <summary>
        /// Adds directives to CLI application.
        /// </summary>
        /// <param name="cliBuilder"></param>
        /// <param name="scanner"></param>
        /// <returns></returns>
        public static ICliBuilder AddDirectives(this ICliBuilder cliBuilder, Action<IScanner<IDirective>> scanner)
        {
            return cliBuilder.GetOrAddScanner<IDirective>(
                services =>
                {
                    return new DirectiveScanner(services);
                },
                scanner);
        }

        /// <summary>
        /// Adds pipelined directives to CLI application.
        /// </summary>
        /// <param name="cliBuilder"></param>
        /// <param name="scanner"></param>
        /// <returns></returns>
        public static ICliBuilder AddPipelinedDirectives(this ICliBuilder cliBuilder, Action<IScanner<IPipelinedDirective>> scanner)
        {
            return cliBuilder.GetOrAddScanner<IPipelinedDirective>(
                services =>
                {
                    return new PipelinedDirectiveScanner(services);
                }, scanner);
        }

        /// <summary>
        /// Adds modes to CLI application.
        /// </summary>
        /// <param name="cliBuilder"></param>
        /// <param name="scanner"></param>
        /// <returns></returns>
        public static ICliBuilder AddModes(this ICliBuilder cliBuilder, Action<IScanner<ICliMode>> scanner)
        {
            return cliBuilder.GetOrAddScanner<ICliMode>(
                services =>
                {
                    return new CliModeScanner(services);
                }, scanner);
        }
    }
}