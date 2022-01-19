namespace SimpleAppExample.Commands
{
    using System.Text.Json;
    using System.Threading;
    using System.Threading.Tasks;
    using Typin.Commands;
    using Typin.Commands.Builders;
    using Typin.Console;
    using Typin.Models;
    using Typin.Models.Builders;
    using Typin.Schemas.Attributes;
    using Typin.Schemas.Builders;

    [Alias]
    public sealed record FluentSampleCommand : ICommand
    {
        public int? Parameter { get; init; }
        public string? ReqStrOption { get; init; }
        public string? StrOption { get; init; }
        public int IntOption { get; init; }
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
                builder.AddDefaultAlias()
                    .UseDescription("A command configured using fluent API.");

                return default;
            }
        }
    }
}