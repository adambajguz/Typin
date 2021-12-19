﻿namespace InteractiveModeExample.Commands
{
    using System;
    using System.Threading.Tasks;
    using Typin;
    using Typin.Attributes;
    using Typin.Console;

    [Command("test", Description = "Test command.")]
    public class TestCommand : ICommand
    {
        [CommandOption("xe", 'a')]
        public string Author { get; init; } = string.Empty;

        [CommandOption('x')]
        public string AuthorX { get; init; } = string.Empty;

        [CommandOption("char", 'c')]
        public char Ch { get; init; }

        [CommandOption("date", 'd')]
        public DateTime Date { get; init; } = DateTime.Now;

        public ValueTask ExecuteAsync(IConsole console)
        {
            console.Output.WriteLine($"'{Author}' '{AuthorX}' '{Ch}'");

            return default;
        }
    }
}
