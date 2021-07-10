namespace Typin.DynamicCommands
{
    /// <summary>
    /// Dynamic command builder factory.
    /// </summary>
    public interface IDynamicCommandBuilderFactory
    {
        /// <summary>
        /// Creates a new <see cref="IDynamicCommandBuilder"/> instance.
        /// </summary>
        /// <returns></returns>
        IDynamicCommandBuilder Create();
    }
}
