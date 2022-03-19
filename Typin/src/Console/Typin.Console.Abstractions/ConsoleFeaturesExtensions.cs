namespace Typin.Console
{
    using System;

    /// <summary>
    /// Console features extensions.
    /// </summary>
    public static class ConsoleFeaturesExtensions
    {
        /// <summary>
        /// Checks whether feature is enabled.
        /// </summary>
        /// <param name="console"></param>
        /// <param name="feature"></param>
        /// <exception cref="NotSupportedException">Throws when one or more requested features are not supported.</exception>
        public static bool IsSupported(this IConsole console, ConsoleFeatures feature)
        {
            return console.SupportedFeatures.HasFlag(feature);
        }

        /// <summary>
        /// Checks whether feature is enabled.
        /// </summary>
        /// <param name="console"></param>
        /// <param name="feature"></param>
        /// <exception cref="NotSupportedException">Throws when one or more requested features are not supported.</exception>
        public static bool IsEnabled(this IConsole console, ConsoleFeatures feature)
        {
            return console.EnabledFeatures.HasFlag(feature);
        }

        /// <summary>
        /// Enables console features.
        ///
        /// This is an equivalent of setting <see cref="IConsole.EnabledFeatures"/>.
        /// </summary>
        /// <param name="console"></param>
        /// <param name="consoleFeatures"></param>
        /// <exception cref="NotSupportedException">Throws when one or more requested features are not supported.</exception>
        public static void Enable(this IConsole console, ConsoleFeatures consoleFeatures)
        {
            console.EnabledFeatures |= consoleFeatures;
        }

        /// <summary>
        /// Tries to enable console features.
        ///
        /// This is an equivalent of setting <see cref="IConsole.EnabledFeatures"/>.
        /// </summary>
        /// <param name="console"></param>
        /// <param name="consoleFeatures"></param>
        /// <returns>True when all features were enabled, false when one or more requested features are not supported.</returns>
        public static bool TryEnable(this IConsole console, ConsoleFeatures consoleFeatures)
        {
            if ((console.SupportedFeatures | consoleFeatures) != console.SupportedFeatures)
            {
                return false;
            }

            console.EnabledFeatures |= consoleFeatures;

            return true;
        }

        /// <summary>
        /// Enables console features.
        ///
        /// This is an equivalent of setting <see cref="IConsole.EnabledFeatures"/>.
        /// </summary>
        /// <param name="console"></param>
        /// <param name="consoleFeatures"></param>
        /// <exception cref="NotSupportedException">Throws when one or more requested features are not enabled.</exception>
        public static void Disable(this IConsole console, ConsoleFeatures consoleFeatures)
        {
            console.EnabledFeatures &= ~consoleFeatures;
        }

        /// <summary>
        /// Resets console features.
        ///
        /// This is an equivalent of setting <see cref="IConsole.EnabledFeatures"/> to <see cref="IConsole.SupportedFeatures"/>.
        /// </summary>
        /// <param name="console"></param>
        public static void ResetFeatures(this IConsole console)
        {
            console.EnabledFeatures = console.SupportedFeatures;
        }
    }
}
