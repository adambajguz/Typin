namespace Typin.Directives
{
    /// <summary>
    /// Directive handler arguments.
    /// </summary>
    public interface IDirectiveArgs<TDirective> : IDirectiveArgs
        where TDirective : class, IDirective
    {
        /// <summary>
        /// Directive data.
        /// </summary>
        new TDirective Directive { get; }
    }
}
