using Typin.DynamicCommands;

namespace Typin.Schemas
{
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
        /// Dynamic argument accesible throught <see cref="DynamicArgumentCollection"/>.
        /// </summary>
        Dynamic,

        /// <summary>
        /// Special built-in argument.
        /// </summary>
        BuiltIn
    }
}