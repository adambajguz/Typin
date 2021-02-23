namespace Typin
{
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Pipelined directive handler.
    /// </summary>
    public interface IPipelinedDirective : IDirective
    {
        /// <summary>
        /// Executes the handler using the curent instance of <see cref="ICliContext"/>.
        /// This is the method that's called when a directive is specified by a user through command line.
        /// Perform any additional behavior and await the next delegate as necessary.
        /// If you happen not to execute next delegate, do not forget to set ExitCode because otherwise null will be propageted and replaced with <see cref="ExitCodes.Error"/> by <see cref="ICliCommandExecutor"/> (unless you want this).
        /// </summary>
        ValueTask HandleAsync(ICliContext context, CommandPipelineHandlerDelegate next, CancellationToken cancellationToken);
    }
}