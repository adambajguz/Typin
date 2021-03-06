﻿namespace InteractiveModeExample.Commands
{
    using System.Threading.Tasks;
    using Typin;
    using Typin.Attributes;
    using Typin.Console;
    using Typin.Modes;

    [Command("quit", Description = "Quits the interactive mode",
        SupportedModes = new[] { typeof(InteractiveMode) })]
    public class QuitCommand : ICommand
    {
        private readonly ICliApplicationLifetime _applicationLifetime;

        public QuitCommand(ICliApplicationLifetime applicationLifetime)
        {
            _applicationLifetime = applicationLifetime;
        }

        public ValueTask ExecuteAsync(IConsole console)
        {
            _applicationLifetime.RequestStop();

            return default;
        }
    }
}