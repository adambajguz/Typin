namespace Typin.Internal.Exceptions
{
    using System;
    using System.Text;
    using Typin.AutoCompletion;
    using Typin.Exceptions;
    using Typin.Schemas;

    /// <summary>
    /// End-user-facing exceptions. Avoid internal details and fix recommendations here
    /// </summary>
    internal static class ModeEndUserExceptions
    {
        public static TypinException DuplicatedShortcut(ShortcutDefinition definition)
        {
            string message = $@"Shortcut '{definition.Modifiers}+{definition.Key}' is already used by Typin.";

            return new TypinException(message);
        }

        public static TypinException InvalidStartupModeType(Type type)
        {
            var message = $"Cannot start the app. '{type.FullName}' is not a valid CLI mode type.";

            return new TypinException(message);
        }

        public static TypinException CommandExecutedInInvalidMode(CommandSchema command, Type currentMode)
        {
            var builder = new StringBuilder();
            builder.AppendLine($"This application is running in '{currentMode}' mode.");

            if (command.SupportedModes != null)
            {
                builder.AppendLine();
                builder.AppendLine($"Command '{command.Type.FullName}' supports modes:");

                foreach (Type mode in command.SupportedModes!)
                {
                    builder.AppendLine($"  - '{mode.FullName}'");
                }
            }

            if (command.ExcludedModes != null)
            {
                builder.AppendLine();
                builder.AppendLine($"Command '{command.Type.FullName}' cannot run in modes:");

                foreach (Type mode in command.ExcludedModes!)
                {
                    builder.AppendLine($"  - '{mode.FullName}'");
                }
            }

            return new TypinException(builder.ToString());
        }

        public static TypinException DirectiveExecutedInInvalidMode(DirectiveSchema directive, Type currentMode)
        {
            var builder = new StringBuilder();
            builder.AppendLine($"This application is running in '{currentMode}' mode.");
            builder.AppendLine($"However, directive '{directive.Type.FullName}' can be executed only from the following modes:");

            foreach (Type mode in directive.SupportedModes!)
            {
                builder.AppendLine($"  - '{mode.FullName}'");
            }

            return new TypinException(builder.ToString());
        }
    }
}