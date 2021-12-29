namespace SimpleAppExample.Commands
{
    using System.Text.Json;
    using System.Threading;
    using System.Threading.Tasks;
    using Typin.Attributes;
    using Typin.Commands;
    using Typin.Commands.Builders;
    using Typin.Console;
    using Typin.Models;
    using Typin.Models.Builders;

    [Command]
    public sealed record FluentSampleCommand : ICommand
    {
        [Parameter(0)]
        public int? Parameter { get; init; }

        [Option("req-str", 's', IsRequired = true)]
        public string? ReqStrOption { get; init; }

        [Option("str", 's')]
        public string? StrOption { get; init; }

        [Option("int", 'i')]
        public int IntOption { get; init; }

        [Option("bool", 'b')]
        public bool BoolOption { get; init; }

        private sealed class Handler : ICommandHandler<FluentSampleCommand>
        {
            private readonly IConsole _console;

            public Handler(IConsole console)
            {
                _console = console;
            }

            public async ValueTask ExecuteAsync(FluentSampleCommand command, CancellationToken cancellationToken)
            {
                await _console.Output.WriteLineAsync(JsonSerializer.Serialize(command));
            }
        }

        private sealed class Configure : IConfigureModel<FluentSampleCommand>, IConfigureCommand<FluentSampleCommand>
        {
            public ValueTask ConfigureAsync(IModelBuilder<FluentSampleCommand> builder, CancellationToken cancellationToken)
            {
                builder.Parameter(x => x.Parameter)
                    .Order(0)
                    .Description("INT parameter.");

                builder.Option(x => x.ReqStrOption)
                    .Name("req-str")
                    .ShortName('r')
                    .IsRequired();

                builder.Option(x => x.StrOption)
                    .Name("str")
                    .ShortName('s');

                builder.Option(x => x.BoolOption)
                    .Name("bool")
                    .ShortName('b');

                return default;
            }

            public ValueTask ConfigureAsync(ICommandBuilder<FluentSampleCommand> builder, CancellationToken cancellationToken)
            {
                builder.DefaultName()
                    .Description("A command configured using fluent API.");

                return default;
            }
        }
    }
}