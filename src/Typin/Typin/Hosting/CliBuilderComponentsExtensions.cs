namespace Typin.Hosting
{
    using System;
    using Typin.Commands;
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
        public static ICliBuilder AddCommands(this ICliBuilder cliBuilder, Action<ICommandScanner> scanner)
        {
            return cliBuilder.GetOrAddScanner<ICommand, ICommandScanner>(
                b =>
                {
                    return new CommandScanner(b.Services);
                }, scanner);
        }

        /// <summary>
        /// Adds commands to CLI application.
        /// </summary>
        /// <param name="cliBuilder"></param>
        /// <param name="scanner"></param>
        /// <returns></returns>
        public static ICliBuilder AddDynamicCommands(this ICliBuilder cliBuilder, Action<IDynamicCommandScanner> scanner)
        {
            return cliBuilder.GetOrAddScanner<ICommandTemplate, IDynamicCommandScanner>(
                b =>
                {
                    return new DynamicCommandScanner(b.Services);
                }, scanner);
        }

        /// <summary>
        /// Adds directives to CLI application.
        /// </summary>
        /// <param name="cliBuilder"></param>
        /// <param name="scanner"></param>
        /// <returns></returns>
        public static ICliBuilder AddDirectives(this ICliBuilder cliBuilder, Action<IDirectiveScanner> scanner)
        {
            return cliBuilder.GetOrAddScanner<IDirective, IDirectiveScanner>(
                b =>
                {
                    return new DirectiveScanner(b.Services);
                },
                scanner);
        }

        /// <summary>
        /// Adds pipelined directives to CLI application.
        /// </summary>
        /// <param name="cliBuilder"></param>
        /// <param name="scanner"></param>
        /// <returns></returns>
        public static ICliBuilder AddPipelinedDirectives(this ICliBuilder cliBuilder, Action<IPipelinedDirectiveScanner> scanner)
        {
            return cliBuilder.GetOrAddScanner<IPipelinedDirective, IPipelinedDirectiveScanner>(
                b =>
                {
                    return new PipelinedDirectiveScanner(b.Services);
                }, scanner);
        }

        /// <summary>
        /// Adds modes to CLI application.
        /// </summary>
        /// <param name="cliBuilder"></param>
        /// <param name="scanner"></param>
        /// <returns></returns>
        public static ICliBuilder AddModes(this ICliBuilder cliBuilder, Action<ICliModeScanner> scanner)
        {
            return cliBuilder.GetOrAddScanner<ICliMode, ICliModeScanner>(
                b =>
                {
                    return new CliModeScanner(b.Services);
                }, scanner);
        }
    }
}