namespace Typin.Binding
{
    /// <summary>
    /// Base type for strongly-typed custom converters.
    /// </summary>
    public abstract class BindingConverter<T> : IBindingConverter
    {
        /// <summary>
        /// Converts raw command line input to <typeparamref name="T"/>.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        object? IBindingConverter.Convert(string? value)
        {
            return Convert(value);
        }

        /// <summary>
        /// Converts raw command line input to <typeparamref name="T"/>.
        /// </summary>
        public abstract T? Convert(string? value);
    }
}
