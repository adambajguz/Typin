namespace TypinExamples.HelloWorld.Commands
{
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Typin;
    using Typin.Attributes;
    using Typin.Console;

    [Command(Description = "Simple command that prints text.")]
    public class SimpleCommand : ICommand
    {
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
            _logger.LogTrace("Hello world!");
            _logger.LogDebug("Hello world!");
            _logger.LogInformation("Hello world!");
            _logger.LogWarning("Hello world!");
            _logger.LogError("Hello world!");
            _logger.LogCritical("Hello world!");

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