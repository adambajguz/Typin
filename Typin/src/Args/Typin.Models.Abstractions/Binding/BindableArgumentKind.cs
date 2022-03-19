namespace Typin.Models.Binding
{
    using Typin.Models.Collections;

    /// <summary>
    /// <see cref="IBindableArgument"/> kind.
    /// </summary>
    public enum BindableArgumentKind
    {
        /// <summary>
        /// Property based bindable argument.
        /// </summary>
        Property,

        /// <summary>
        /// Dynamic argument accessible through <see cref="IArgumentCollection"/>.
        /// </summary>
        Dynamic
    }
}