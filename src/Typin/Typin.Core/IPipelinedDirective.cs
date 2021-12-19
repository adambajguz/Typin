namespace Typin
{
    using PackSite.Library.Pipelining;

    /// <summary>
    /// Pipelined directive handler.
    ///
    /// Perform any additional behavior and await the next delegate as necessary.
    /// If you happen not to execute next delegate, do not forget to set ExitCode because otherwise null
    /// will be propageted and replaced with <see cref="ExitCode.Error"/> by <see cref="ICliModeSwitcher"/> (unless you want this).
    /// </summary>
    public interface IPipelinedDirective : IDirective, IStep<CliContext>
    {

    }
}