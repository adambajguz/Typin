namespace Typin.Directives
{
    /// <summary>
    /// Directive handler arguments.
    /// </summary>
    public interface IDirectiveArgs
    {
        /// <summary>
        /// Directive data.
        /// </summary>
        IDirective Directive { get; }

        /// <summary>
        /// Current CLI context.
        /// </summary>
        CliContext Context { get; }
    }
}
