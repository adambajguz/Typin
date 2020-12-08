namespace Typin.Modes
{
    using System;
    using System.Collections.Generic;
    using Typin.AutoCompletion;

    /// <summary>
    /// Interactive mode options.
    /// </summary>
    public class InteractiveModeOptions
    {
        /// <summary>
        /// Prompt foreground color.
        /// Default color is <see cref="ConsoleColor.Blue"/>.
        /// </summary>
        public ConsoleColor PromptForeground { get; set; } = ConsoleColor.Blue;

        /// <summary>
        /// Command input foreground color.
        /// Default color is <see cref="ConsoleColor.Yellow"/>.
        /// </summary>
        public ConsoleColor CommandForeground { get; set; } = ConsoleColor.Yellow;

        /// <summary>
        /// Command scope.
        /// </summary>
        public string Scope { get; set; } = string.Empty;

        /// <summary>
        /// User defined shortcuts.
        /// </summary>
        public HashSet<ShortcutDefinition> UserDefinedShortcuts { get; set; } = new HashSet<ShortcutDefinition>();

        /// <summary>
        /// Whether advanced input is available.
        /// </summary>
        public bool IsAdvancedInputAvailable { get; set; } = true;
    }
}
