//namespace Typin.Tests.Data.Valid.Commands
//{
//    using System;
//    using System.Threading;
//    using System.Threading.Tasks;
//    using Typin;
//    using Typin.Attributes;
//    using Typin.Commands;
//    using Typin.Commands.Schemas;
//    using Typin.Console;
//    using Typin.Schemas;
//    using Typin.Tests.Data.Valid.DynamicCommands;

//    [Command("add valid-dynamic-and-execute", Description = "Adds a dynamic command and executed it.")]
//    public class AddValidDynamicAndExecuteCommand : ICommand
//    {
//        private readonly IConsole _console;
//        private readonly IDynamicCommandBuilderFactory _dynamicCommandBuilderFactory;
//        private readonly RootSchema _rootSchema;
//        private readonly ICommandExecutor _commandExecutor;

//        [Option("name")]
//        public string Name { get; init; } = string.Empty;

//        public AddValidDynamicAndExecuteCommand(IConsole console,
//                                                IDynamicCommandBuilderFactory dynamicCommandBuilderFactory,
//                                                IRootSchemaAccessor rootSchemaAccessor,
//                                                ICommandExecutor commandExecutor)
//        {
//            _console = console;
//            _dynamicCommandBuilderFactory = dynamicCommandBuilderFactory;
//            _rootSchema = rootSchemaAccessor.RootSchema;
//            _commandExecutor = commandExecutor;
//        }

//        public async ValueTask ExecuteAsync(CancellationToken cancellationToken)
//        {
//            ICommandSchema commandSchema = _dynamicCommandBuilderFactory.Create<ValidDynamicCommand>(Name)
//                .WithDescription("Test description.")
//                .WithManual("Some manual\nadd dynamic --name abc\nabc 5 j --number 4 -a aaaaaa\nabc --help.")
//                .AddOption<int>("Number", (ob) => ob
//                    .AsRequired()
//                    .WithDescription("Some number.")
//                )
//                .AddOption(typeof(double))
//                .AddOption<int?>()
//                .AddOption<string>("Str")
//                .AddOption(typeof(double), "Price")
//                .AddParameter<string>("Parameter", 0)
//                .AddParameter<string>(1)
//                .Build();

//            if (_rootSchema.TryAddCommand(commandSchema))
//            {
//                _console.Output.WithForegroundColor(ConsoleColor.Green, (err) => err.WriteLine($"Successfully added dynamic command '{Name}'."));
//            }
//            else
//            {
//                _console.Error.WithForegroundColor(ConsoleColor.Red, (err) => err.WriteLine($"Failed to add dynamic command '{Name}'."));
//            }

//            await _commandExecutor.ExecuteAsync(new string[] { Name, "test1", "test2", "0", "--number", "2", "--price", "4.65" }, cancellationToken: cancellationToken);
//            await _commandExecutor.ExecuteAsync($"{Name} abc def 1 --number 10 --price 0", cancellationToken: cancellationToken);
//        }
//    }
//}
