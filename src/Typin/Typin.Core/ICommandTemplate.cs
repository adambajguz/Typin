namespace Typin
{
    using Typin.Models;

    /// <summary>
    /// Represents a command template that has an entry point.
    /// </summary>
    public interface ICommandTemplate : ICommand, IDynamicModel
    {
        //TODO: Add ICommandHandler
    }
}