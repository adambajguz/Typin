namespace Typin.Binding
{
    /// <summary>
    /// Internal type for custom converters.
    /// </summary>
    internal interface IBindingConverter
    {
        /// <summary>
        /// Converts raw command line input to <see cref="object"/>.
        /// This method is used by argument binder.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        object? Convert(string? value);
    }
}
