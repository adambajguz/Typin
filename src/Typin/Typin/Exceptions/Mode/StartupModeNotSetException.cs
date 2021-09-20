namespace Typin.Exceptions.Mode
{
    /// <summary>
    /// Startup mode not set exception.
    /// </summary>
    public sealed class StartupModeNotSetException : ModeException
    {
        /// <summary>
        /// Initializes an instance of <see cref="NoModeException"/>.
        /// </summary>
        public StartupModeNotSetException() :
            base(BuildMessage())
        {

        }

        private static string BuildMessage()
        {
            return "Cannot start CLI: startup mode not set.";
        }
    }
}