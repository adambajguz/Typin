namespace Typin.Console
{
    using System;
    using Typin.Console.IO;

    /// <summary>
    /// Extensions for <see cref="IConsole"/>.
    /// </summary>
    public static class ConsoleExtensions
    {
        /// <summary>
        /// Sets console foreground color, executes specified action, and sets the color back to the original value.
        /// </summary>
        public static void WithForegroundColor(this StandardStreamWriter stream, ConsoleColor foregroundColor, Action<StandardStreamWriter> action)
        {
            IConsole console = stream.BoundedConsole;

            ConsoleColor lastForegroundColor = console.ForegroundColor;
            console.ForegroundColor = foregroundColor;

            action(stream);

            console.ForegroundColor = lastForegroundColor;
        }

        /// <summary>
        /// Sets console background color, executes specified action, and sets the color back to the original value.
        /// </summary>
        public static void WithBackgroundColor(this StandardStreamWriter stream, ConsoleColor backgroundColor, Action<StandardStreamWriter> action)
        {
            IConsole console = stream.BoundedConsole;

            ConsoleColor lastBackgroundColor = console.BackgroundColor;
            console.BackgroundColor = backgroundColor;

            action(stream);

            console.BackgroundColor = lastBackgroundColor;
        }

        /// <summary>
        /// Sets console foreground and background colors, executes specified action, and sets the colors back to the original values.
        /// </summary>
        public static void WithColors(this StandardStreamWriter stream, ConsoleColor foregroundColor, ConsoleColor backgroundColor, Action<StandardStreamWriter> action)
        {
            IConsole console = stream.BoundedConsole;

            ConsoleColor lastForegroundColor = console.ForegroundColor;
            ConsoleColor lastBackgroundColor = console.BackgroundColor;
            console.BackgroundColor = backgroundColor;
            console.ForegroundColor = foregroundColor;

            action(stream);

            console.ForegroundColor = lastForegroundColor;
            console.BackgroundColor = lastBackgroundColor;
        }

        /// <summary>
        /// Sets console foreground color, executes specified action, and sets the color back to the original value.
        /// </summary>
        public static void WithForegroundColor(this IConsole console, ConsoleColor foregroundColor, Action<IStandardOutputAndError> action)
        {
            ConsoleColor lastForegroundColor = console.ForegroundColor;
            console.ForegroundColor = foregroundColor;

            action(console);

            console.ForegroundColor = lastForegroundColor;
        }

        /// <summary>
        /// Sets console background color, executes specified action, and sets the color back to the original value.
        /// </summary>
        public static void WithBackgroundColor(this IConsole console, ConsoleColor backgroundColor, Action<IStandardOutputAndError> action)
        {
            ConsoleColor lastBackgroundColor = console.BackgroundColor;
            console.BackgroundColor = backgroundColor;

            action(console);

            console.BackgroundColor = lastBackgroundColor;
        }

        /// <summary>
        /// Sets console foreground and background colors, executes specified action, and sets the colors back to the original values.
        /// </summary>
        public static void WithColors(this IConsole console, ConsoleColor foregroundColor, ConsoleColor backgroundColor, Action<IStandardOutputAndError> action)
        {
            ConsoleColor lastForegroundColor = console.ForegroundColor;
            ConsoleColor lastBackgroundColor = console.BackgroundColor;
            console.BackgroundColor = backgroundColor;
            console.ForegroundColor = foregroundColor;

            action(console);

            console.ForegroundColor = lastForegroundColor;
            console.BackgroundColor = lastBackgroundColor;
        }
    }
}