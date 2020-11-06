namespace Typin.Internal.Exceptions
{
    using System;
    using Typin.Exceptions;

    /// <summary>
    /// End-user-facing exceptions. Avoid internal details and fix recommendations here
    /// </summary>
    internal static class ModeSwitchingExceptions
    {
        internal static TypinException InvalidModeType(Type type)
        {
            var message = $"'{type.FullName}' is not a valid CLI mode type.";

            return new TypinException(message.Trim());
        }
    }
}