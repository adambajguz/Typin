namespace Typin.Internal.Exceptions
{
    using Typin.Directives;
    using Typin.Exceptions;
    using Typin.Input;
    using Typin.Schemas;

    /// <summary>
    /// Interactive mode end-user-facing exceptions. Avoid internal details and fix recommendations here
    /// </summary>
    internal static class InteractiveModeEndUserExceptions

    {
        internal static TypinException InteractiveOnlyCommandButThisIsDirectApplication(CommandSchema command)
        {
            var message = $@"
Command '{command.Type.FullName}' can be executed only in interactive mode, but this application is using {nameof(CliApplication)}.

Please consider switching to interactive application or removing the command.";

            return new TypinException(message.Trim());
        }

        internal static TypinException UnknownDirectiveName(DirectiveInput directive)
        {
            var message = $@"
Unknown directive '{directive}'.";

            return new TypinException(message.Trim());
        }

        internal static TypinException InteractiveOnlyCommandButInteractiveModeNotStarted(CommandSchema command)
        {
            var message = $@"
Command '{command.Type.FullName}' can be executed only in interactive mode, but this application is not running in this mode.

You can start the interactive mode with [{BuiltInDirectives.Interactive}].";

            return new TypinException(message.Trim());
        }

        internal static TypinException InteractiveModeNotSupported()
        {
            var message = $@"
This application does not support interactive mode.";

            return new TypinException(message.Trim());
        }

        internal static TypinException InteractiveModeDirectiveNotAvailable(string directiveName)
        {
            var message = $@"
Directive '[{directiveName}]' is for interactive mode only. Thus, cannot be used in direct mode.

You can start the interactive mode with [{BuiltInDirectives.Interactive}].";

            return new TypinException(message.Trim());
        }
    }
}