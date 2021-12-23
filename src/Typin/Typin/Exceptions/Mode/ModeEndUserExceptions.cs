﻿namespace Typin.Exceptions.Mode
{
    using System;
    using System.Text;
    using Typin.Internal.Exceptions;
    using Typin.Schemas;

    /// <summary>
    /// End-user-facing exceptions. Avoid internal details and fix recommendations here
    /// </summary>
    internal static class ModeEndUserExceptions
    {
        public static TypinException InvalidStartupModeType(Type type)
        {
            var message = $"Cannot start the app. '{type.FullName}' is not a valid CLI mode type.";

            return new TemporaryException(message);
        }

        //public static TypinException CommandExecutedInInvalidMode(CommandSchema command, Type currentMode)
        //{
        //    StringBuilder builder = new();
        //    builder.AppendLine($"This application is running in '{currentMode}' mode.");

        //    if (command.SupportedModes is not null)
        //    {
        //        builder.AppendLine();
        //        builder.AppendLine($"Command '{command.Type.FullName}' supports modes:");

        //        foreach (Type mode in command.SupportedModes)
        //        {
        //            builder.AppendLine($"  - '{mode.FullName}'");
        //        }
        //    }

        //    if (command.ExcludedModes is not null)
        //    {
        //        builder.AppendLine();
        //        builder.AppendLine($"Command '{command.Type.FullName}' cannot run in modes:");

        //        foreach (Type mode in command.ExcludedModes)
        //        {
        //            builder.AppendLine($"  - '{mode.FullName}'");
        //        }
        //    }

        //    return new TemporaryException(builder.ToString());
        //}

        public static TypinException DirectiveExecutedInInvalidMode(DirectiveSchema directive, Type currentMode)
        {
            StringBuilder builder = new();
            builder.AppendLine($"This application is running in '{currentMode}' mode.");

            if (directive.SupportedModes is not null)
            {
                builder.AppendLine();
                builder.AppendLine($"Directive '{directive.Type.FullName}' supports modes:");

                foreach (Type mode in directive.SupportedModes)
                {
                    builder.AppendLine($"  - '{mode.FullName}'");
                }
            }

            if (directive.ExcludedModes is not null)
            {
                builder.AppendLine();
                builder.AppendLine($"Directive '{directive.Type.FullName}' cannot run in modes:");

                foreach (Type mode in directive.ExcludedModes)
                {
                    builder.AppendLine($"  - '{mode.FullName}'");
                }
            }

            return new TemporaryException(builder.ToString());
        }
    }
}