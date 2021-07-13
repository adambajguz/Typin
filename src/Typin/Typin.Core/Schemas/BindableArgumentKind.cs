namespace Typin.Schemas
{
    using Typin.DynamicCommands;

    /// <summary>
    /// <see cref="BindableArgument"/> kind.
    /// </summary>
    public enum BindableArgumentKind
    {
        /// <summary>
        /// Property based bindable arguemnt.
        /// </summary>
        Property,

        /// <summary>
        /// Dynamic argument accesible throught <see cref="IArgumentCollection"/>.
        /// </summary>
        Dynamic,

        /// <summary>
        /// Special built-in argument.
        /// </summary>
        BuiltIn
    }
}