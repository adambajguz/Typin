namespace Typin
{
    using Typin.DynamicCommands;

    /// <summary>
    /// Entry point of a dynamic command.
    /// </summary>
    public interface IDynamicCommand : ICommand
    {
        /// <summary>
        /// Dynamic command arguments.
        /// </summary>
        IDynamicArgumentCollection Arguments { get; init; }
    }
}