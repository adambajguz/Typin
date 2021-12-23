namespace Typin.Models.Binding
{
    using Typin.Models.Collections;

    /// <summary>
    /// <see cref="IBindableArgument"/> kind.
    /// </summary>
    public enum BindableArgumentKind
    {
        /// <summary>
        /// Property based bindable arguemnt.
        /// </summary>
        Property,

        /// <summary>
        /// Dynamic argument accessible throught <see cref="IArgumentCollection"/>.
        /// </summary>
        Dynamic
    }
}