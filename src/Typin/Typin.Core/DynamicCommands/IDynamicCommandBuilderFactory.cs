namespace Typin.DynamicCommands
{
    using System;

    /// <summary>
    /// Dynamic command builder factory.
    /// </summary>
    public interface IDynamicCommandBuilderFactory
    {
        /// <summary>
        /// Creates a new <see cref="IDynamicCommandBuilder"/> instance for a command with name specified in <paramref name="commandName"/>.
        /// </summary>
        /// <param name="dynamicCommandType"></param>
        /// <param name="commandName"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Throws when <paramref name="commandName"/> is null or whitespace.</exception>
        /// <exception cref="ArgumentException">Throws when <paramref name="dynamicCommandType"/> is does not represent a dynamic command.</exception>
        IDynamicCommandBuilder Create(Type dynamicCommandType, string commandName);

        /// <summary>
        /// Creates a new <see cref="IDynamicCommandBuilder"/> instance for a command with name specified in <paramref name="commandName"/>.
        /// </summary>
        /// <param name="commandName"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Throws when <paramref name="commandName"/> is null or whitespace.</exception>
        IDynamicCommandBuilder Create<T>(string commandName)
            where T : class, ICommandTemplate;
    }
}
