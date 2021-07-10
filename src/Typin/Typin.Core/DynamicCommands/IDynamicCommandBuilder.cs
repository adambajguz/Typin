namespace Typin.DynamicCommands
{
    using System;

    /// <summary>
    /// Dynamic command builder.
    /// </summary>
    public interface IDynamicCommandBuilder
    {
        /// <summary>
        /// Sets dynamic command name.
        /// If the name is not set, the command is treated as a default command, i.e. the one that gets executed when the user
        /// does not specify a command name in the arguments.
        /// All commands in an application must have different names. Likewise, only one command without a name is allowed.
        /// </summary>
        /// <param name="name"></param>
        IDynamicCommandBuilder WithName(string? name);

        /// <summary>
        /// Sets dynamic command description, which is used in help text.
        /// </summary>
        /// <param name="description"></param>
        IDynamicCommandBuilder WithDescription(string? description);

        /// <summary>
        /// Sets dynamic command manual text, which is used in help text.
        /// </summary>
        /// <param name="manual"></param>
        IDynamicCommandBuilder WithManual(string? manual);

        /// <summary>
        /// List of CLI mode types, in which the dynamic command can be executed.
        /// If null (default) or empty, command can be executed in every registered mode in the app.
        /// </summary>
        /// <param name="supportedModes"></param>
        IDynamicCommandBuilder WithSupportedModes(params Type[] supportedModes);

        /// <summary>
        /// List of CLI mode types, in which the dynamic command cannot be executed.
        /// If null (default) or empty, command can be executed in every registered mode in the app.
        /// </summary>
        /// <param name="excludedModes"></param>
        IDynamicCommandBuilder WithExcludedModes(params Type[] excludedModes);

        /// <summary>
        /// Adds an option with type specified by <paramref name="optionType"/>.
        /// </summary>
        /// <param name="optionType"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        IDynamicCommandBuilder AddOption(Type optionType, Action<IDynamicOptionBuilder> action);

        /// <summary>
        /// Adds an option with type specified by <typeparamref name="T"/>.
        /// </summary>
        /// <param name="action"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IDynamicCommandBuilder AddOption<T>(Action<IDynamicOptionBuilder<T>> action);

        /// <summary>
        /// Adds a parameter with type specified by <paramref name="parameterType"/>.
        ///
        /// Order of this parameter compared to other parameters.
        /// All parameters in a command must have different order.
        /// Parameter whose type is a non-scalar (e.g. array), must be the last in order and only one such parameter is allowed.
        /// </summary>
        /// <param name="parameterType"></param>
        /// <param name="order"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        IDynamicCommandBuilder AddParameter(Type parameterType, int order, Action<IDynamicParameterBuilder> action);

        /// <summary>
        /// Adds a parameter with type specified by <typeparamref name="T"/>.
        ///
        /// Order of this parameter compared to other parameters.
        /// All parameters in a command must have different order.
        /// Parameter whose type is a non-scalar (e.g. array), must be the last in order and only one such parameter is allowed.
        /// </summary>
        /// <param name="action"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IDynamicCommandBuilder AddParameter<T>(int order, Action<IDynamicParameterBuilder<T>> action);
    }
}
