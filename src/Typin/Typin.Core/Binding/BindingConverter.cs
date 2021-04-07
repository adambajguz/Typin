namespace Typin.Binding
{
    /// <summary>
    /// Base type for custom converters.
    /// </summary>
    public interface IBindingConverter
    {
        /// <summary>
        /// Converts raw command line input to <see cref="object"/>.
        /// This method is used by argument binder.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        object? Convert(string? value);
    }

    /// <summary>
    /// Base type for strongly-typed custom converters.
    /// </summary>
    public interface IBindingConverter<T> : IBindingConverter
    {
#if !NETSTANDARD2_0
        /// <summary>
        /// Converts raw command line input to <typeparamref name="T"/>.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        object? IBindingConverter.Convert(string? value)
        {
            return Convert(value);
        }
#endif
        
        /// <summary>
        /// Converts raw command line input to <typeparamref name="T"/>.
        /// </summary>
        new T? Convert(string? value);
    }
}
