namespace Typin.DynamicCommands
{
    using System;
    using Typin.Binding;

    /// <summary>
    /// Dynamic command parameter builder.
    /// </summary>
    public interface IDynamicParameterBuilder<T> : IDynamicParameterBuilder
    {
        /// <summary>
        /// Sets a binding converter for this parameter.
        /// </summary>
        IDynamicParameterBuilder<T> WithBindingConverter<TConverter>()
            where TConverter : BindingConverter<T>;
    }

    /// <summary>
    /// Dynamic command parameter builder.
    /// </summary>
    public interface IDynamicParameterBuilder
    {
        /// <summary>
        /// Parameter name, which is only used in help text.
        /// </summary>
        IDynamicParameterBuilder WithName(string? name);

        /// <summary>
        /// Parameter description, which is used in help text.
        /// </summary>
        IDynamicParameterBuilder WithDescription(string? description);

        /// <summary>
        /// Sets a binding converter for this parameter.
        /// </summary>
        IDynamicParameterBuilder WithBindingConverter(Type converter);
    }
}
