﻿namespace InteractiveModeExample.Commands
{
    using System.Threading.Tasks;
    using Typin;
    using Typin.Attributes;
    using Typin.Console;

    [Command("exit", Description = "Exits.")]
    public class ExitCommand : ICommand
    {
        private readonly ICliApplicationLifetime _lifetime;

        public ExitCommand(ICliApplicationLifetime lifetime)
        {
            _lifetime = lifetime;
        }

        public ValueTask ExecuteAsync(IConsole console)
        {
            _lifetime.RequestStop();

            return default;
        }
    }
}
