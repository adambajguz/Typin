namespace SimpleAppExample.Commands
{
    using System.ComponentModel;
    using System.Text.Json;
    using System.Threading;
    using System.Threading.Tasks;
    using Typin.Commands;
    using Typin.Commands.Attributes;
    using Typin.Commands.Builders;
    using Typin.Console;
    using Typin.Models;
    using Typin.Models.Attributes;
    using Typin.Models.Builders;
    using Typin.Schemas.Attributes;

    [Alias("attr")]
    [Description("A command configured using attributes.")]
    public sealed record AttributeSampleCommand : ICommand
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

        private sealed class Handler : ICommandHandler<AttributeSampleCommand>
        {
            private readonly IConsole _console;

            public Handler(IConsole console)
            {
                _console = console;
            }

            public async ValueTask ExecuteAsync(AttributeSampleCommand command, CancellationToken cancellationToken)
            {
                await _console.Output.WriteLineAsync(JsonSerializer.Serialize(command));
            }
        }

        private sealed class Configure : IConfigureModel<AttributeSampleCommand>, IConfigureCommand<AttributeSampleCommand>
        {
            public ValueTask ConfigureAsync(IModelBuilder<AttributeSampleCommand> builder, CancellationToken cancellationToken)
            {
                builder.FromAttributes();

                return default;
            }

            public ValueTask ConfigureAsync(ICommandBuilder<AttributeSampleCommand> builder, CancellationToken cancellationToken)
            {
                builder.FromAttributes();

                return default;
            }
        }
    }
}