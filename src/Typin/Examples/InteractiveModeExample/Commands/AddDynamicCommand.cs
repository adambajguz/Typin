namespace InteractiveModeExample.Commands
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Typin;
    using Typin.Attributes;
    using Typin.Console;
    using Typin.DynamicCommands;
    using Typin.Schemas;

    [Command("add dynamic", Description = "Adds a dynamic command.")]
    public class AddDynamicCommand : ICommand
    {
        private readonly IConsole _console;
        private readonly IDynamicCommandBuilderFactory _dynamicCommandBuilderFactory;
        private readonly RootSchema _rootSchema;

        [Option("name")]
        public string Name { get; init; } = string.Empty;

        public AddDynamicCommand(IConsole console, IDynamicCommandBuilderFactory dynamicCommandBuilderFactory, IRootSchemaAccessor rootSchemaAccessor)
        {
            _console = console;
            _dynamicCommandBuilderFactory = dynamicCommandBuilderFactory;
            _rootSchema = rootSchemaAccessor.RootSchema;
        }

        public ValueTask ExecuteAsync(CancellationToken cancellationToken)
        {
            CommandSchema commandSchema = _dynamicCommandBuilderFactory.Create<SampleDynamicCommand>(Name)
                .WithDescription("Test description.")
                .WithManual("Some manual.")
                .AddOption<int>("number", (ob) => ob
                    .AsRequired()
                    .WithDescription("Some number.")
                )
                .AddOption(typeof(double))
                .AddOption(typeof(double), "Price")
                .AddParameter<string>("sth", 0)
                .Build();

            if (_rootSchema.TryAddDynamicCommand(commandSchema))
            {
                _console.Output.WithForegroundColor(ConsoleColor.Green, (err) => err.WriteLine($"Successfully added dynamic command '{Name}'."));
            }
            else
            {
                _console.Error.WithForegroundColor(ConsoleColor.Red, (err) => err.WriteLine($"Failed to add dynamic command '{Name}'."));
            }

            return default;
        }
    }
}
