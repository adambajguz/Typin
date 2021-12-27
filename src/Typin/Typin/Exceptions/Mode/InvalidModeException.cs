namespace Typin.Exceptions.Mode
{
    using System;

    /// <summary>
    /// Invalid mode exception.
    /// </summary>
    public sealed class InvalidModeException : ModeException
    {
        /// <summary>
        /// Mode type.
        /// </summary>
        public Type Type { get; set; }

        /// <summary>
        /// Initializes an instance of <see cref="InvalidModeException"/>.
        /// </summary>
        /// <param name="type"></param>
        public InvalidModeException(Type type) :
            base(BuildMessage(type))
        {
            Type = type;
        }

        private static string BuildMessage(Type type)
        {
            return $"Mode '{type.FullName}' is not a valid mode type.{Environment.NewLine}" +
                   Environment.NewLine +
                   $"In order to be a valid mode type, it must:{Environment.NewLine}" +
                   $"  - Not be an abstract class{Environment.NewLine}" +
                   $"  - Implement {typeof(ICliMode).FullName}{Environment.NewLine}";
        }
    }
}