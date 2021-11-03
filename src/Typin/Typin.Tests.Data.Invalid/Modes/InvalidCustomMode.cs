namespace Typin.Tests.Data.Modes.Invalid
{
    using Typin.Console;

    public class InvalidCustomMode
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0052:Remove unread private members", Justification = "<Pending>")]
        private readonly IConsole _console;

        public InvalidCustomMode(IConsole console)
        {
            _console = console;
        }
    }
}
