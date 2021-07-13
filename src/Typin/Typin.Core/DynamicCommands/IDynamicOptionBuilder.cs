namespace Typin.DynamicCommands
{
    using System;
    using Typin.Binding;
    using Typin.Metadata;

    /// <summary>
    /// Dynamic command option builder.
    /// </summary>
    public interface IDynamicOptionBuilder<T> : IDynamicOptionBuilder
    {
        /// <summary>
        /// Sets a binding converter for this parameter.
        /// </summary>
        IDynamicOptionBuilder<T> WithBindingConverter<TConverter>()
            where TConverter : BindingConverter<T>;
    }

    /// <summary>
    /// Dynamic command option builder.
    /// </summary>
    public interface IDynamicOptionBuilder
    {
        /// <summary>
        /// Option name (must be longer than a single character). Starting dashes are trimed automatically.
        /// All options in a command must have different names (comparison is case-sensitive).
        /// </summary>
        IDynamicOptionBuilder WithName(string? name);

        /// <summary>
        /// Option short name (single character).
        /// All options in a command must have different short names (comparison is case-sensitive).
        /// </summary>
        IDynamicOptionBuilder WithShortName(char? shortName);

        /// <summary>
        /// Sets an option as an optional option.
        /// </summary>
        IDynamicOptionBuilder AsOptional();

        /// <summary>
        /// Sets an option as a required option.
        /// </summary>
        IDynamicOptionBuilder AsRequired();

        /// <summary>
        /// Sets option required value.
        /// </summary>
        IDynamicOptionBuilder WithRequired(bool isRequired);


        /// <summary>
        /// Option description, which is used in help text.
        /// </summary>
        IDynamicOptionBuilder WithDescription(string? description);

        /// <summary>
        /// Fallback variable that will be used as fallback if no option value is specified.
        /// </summary>
        /// <param name="variableName"></param>
        IDynamicOptionBuilder WithFallbackVariableName(string? variableName);

        /// <summary>
        /// Sets a binding converter for this parameter.
        /// </summary>
        IDynamicOptionBuilder WithBindingConverter(Type converterType);

        /// <summary>
        /// Sets option metadata.
        /// </summary>
        /// <returns></returns>
        IDynamicOptionBuilder SetMetadata(IArgumentMetadata metadata);
    }
}
