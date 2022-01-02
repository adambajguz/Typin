namespace Typin.Exceptions.Mode
{
    /// <summary>
    /// No mode exception.
    /// </summary>
    public sealed class NoModeException : ModeException
    {
        /// <summary>
        /// Initializes an instance of <see cref="NoModeException"/>.
        /// </summary>
        public NoModeException() :
            base(BuildMessage())
        {

        }

        private static string BuildMessage()
        {
            return "No CLI mode available in current context";
        }
    }
}