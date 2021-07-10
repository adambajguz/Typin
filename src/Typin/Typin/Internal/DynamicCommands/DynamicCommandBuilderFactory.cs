namespace Typin.DynamicCommands
{
    /// <summary>
    /// Dynamic command builder factory.
    /// </summary>
    internal class DynamicCommandBuilderFactory : IDynamicCommandBuilderFactory
    {
        /// <summary>
        /// Initializes a new instance of <see cref="DynamicCommandBuilderFactory"/>.
        /// </summary>
        public DynamicCommandBuilderFactory()
        {

        }

        /// <inheritdoc/>
        public IDynamicCommandBuilder Create()
        {
            return new DynamicCommandBuilder();
        }
    }
}
