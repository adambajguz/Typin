//namespace InteractiveModeExample.Commands
//{
//    using System;
//    using System.Threading;
//    using System.Threading.Tasks;
//    using Typin;
//    using Typin.Models.Attributes;
//    using Typin.Commands.Attributes;
//    using Typin.Commands;
//    using Typin.Console;
//    using Typin.Schemas;

//    [Command("remove dynamic", Description = "Removes a dynamic command.")]
//    public class RemoveDynamicCommand : ICommand
//    {
//        private readonly IConsole _console;
//        private readonly RootSchema _rootSchema;

//        [Option("name")]
//        public string Name { get; init; } = string.Empty;

//        public RemoveDynamicCommand(IConsole console, IRootSchemaAccessor rootSchemaAccessor)
//        {
//            _console = console;
//            _rootSchema = rootSchemaAccessor.RootSchema;
//        }

//        public ValueTask ExecuteAsync(CancellationToken cancellationToken)
//        {
//            if (_rootSchema.TryRemoveCommand(Name))
//            {
//                _console.Output.WithForegroundColor(ConsoleColor.Green, (err) => err.WriteLine($"Successfully removed dynamic command '{Name}'."));
//            }
//            else
//            {
//                _console.Error.WithForegroundColor(ConsoleColor.Red, (err) => err.WriteLine($"Failed to remove dynamic command '{Name}'."));
//            }

//            return default;
//        }
//    }
//}
