//namespace SimpleAppExample.Commands
//{
//    using System.Text.Json;
//    using System.Threading;
//    using System.Threading.Tasks;
//    using Typin.Attributes;
//    using Typin.Commands;
//    using Typin.Console;

//    [Command]
//    public class SampleCommand : ICommand
//    {
//        private readonly IConsole _console;

//        [Parameter(0)]
//        public int? ParamB { get; init; }

//        [Option("req-str", 's', IsRequired = true)]
//         public string? ReqStrOption { get; init; }

//        [Option("str", 's')]
//        public string? StrOption { get; init; }

//        [Option("int", 'i')]
//        public int IntOption { get; init; }

//        [Option("bool", 'b')]
//        public bool BoolOption { get; init; }

//        public SampleCommand(IConsole console)
//        {
//            _console = console;
//        }

//        public async ValueTask ExecuteAsync(CancellationToken cancellationToken)
//        {
//            await _console.Output.WriteLineAsync(JsonSerializer.Serialize(this));
//        }
//    }
//}