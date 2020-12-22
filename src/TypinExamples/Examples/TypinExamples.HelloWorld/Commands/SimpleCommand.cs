namespace TypinExamples.HelloWorld.Commands
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Typin;
    using Typin.Attributes;
    using Typin.Console;

    [Command(Description = "Simple command that prints text.")]
    public class SimpleCommand : ICommand
    {
        private static string IntentionalErrorMessage = $"{Environment.NewLine}{Environment.NewLine}DO NOT take this error seriously as it is just an example. Switch to Log Viewer and see all logs.";

        [CommandOption("name", 'n')]
        public string? Name { get; init; }

        [CommandOption("surname", 's')]
        public string? Surname { get; init; }

        [CommandOption("mail", 'm', Description = "Email address")]
        public string? Mail { get; init; }

        [CommandOption("age", 'a', Description = "Age.")]
        public int Age { get; init; }

        [CommandOption("height", Description = "Height.")]
        public double? Height { get; init; } = null;

        private readonly ILogger _logger;

        public SimpleCommand(ILogger<SimpleCommand> logger)
        {
            _logger = logger;
        }

        public async ValueTask ExecuteAsync(IConsole console)
        {
            _logger.LogTrace("Example trace message from Typin.");
            _logger.LogDebug("Example debug message from Typin.");
            _logger.LogInformation("Example inforamtion message from Typin.");
            _logger.LogWarning("Example warning message from Typin.");
            _logger.LogError($"Example error message from Typin.{IntentionalErrorMessage}");
            _logger.LogCritical($"Example critical message from Typin.{IntentionalErrorMessage}");

            if (Name is null && Surname is null)
                await console.Output.WriteLineAsync("Hello World!");
            else if (Mail is null)
            {
                await console.Output.WriteLineAsync($"Welcome {Name} {Surname}!");
                await console.Output.WriteLineAsync($"Age {Age}; Height: {(Height is null ? "unknown" : Height.ToString())}");
            }
            else
            {
                await console.Output.WriteLineAsync($"Welcome {Name} {Surname}!");
                await console.Output.WriteLineAsync($"e-mail: {Mail}; Age {Age}; Height: {(Height is null ? "unknown" : Height.ToString())}");
            }
        }
    }
}