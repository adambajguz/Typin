namespace Typin
{
    /// <summary>
    /// Service that can be used to access CLI context.
    /// </summary>
    public interface ICliContextAccessor
    {
        /// <summary>
        /// Root schema.
        /// </summary>
        CliContext? CliContext { get; set; }
    }
}
