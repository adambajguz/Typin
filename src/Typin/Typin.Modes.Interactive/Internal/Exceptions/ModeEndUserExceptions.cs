namespace Typin.Modes.Interactive.Internal.Exceptions
{
    using Typin.Exceptions;
    using Typin.Internal.Exceptions;
    using Typin.Modes.Interactive.AutoCompletion;

    /// <summary>
    /// End-user-facing exceptions. Avoid internal details and fix recommendations here
    /// </summary>
    internal static class ModeEndUserExceptions
    {
        public static TypinException DuplicatedShortcut(ShortcutDefinition definition)
        {
            string message = $@"Shortcut '{definition.Modifiers}+{definition.Key}' is already used by Typin.";

            return new TemporaryException(message);
        }
    }
}