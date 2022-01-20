namespace SimpleAppExample.Directives
{
    using System.Threading;
    using System.Threading.Tasks;
    using PackSite.Library.Pipelining;
    using Typin.Console;
    using Typin.Directives;
    using Typin.Directives.Builders;
    using Typin.Models;
    using Typin.Models.Builders;
    using Typin.Schemas.Builders;

    public sealed class CustomPreviewDirective : IDirective
    {
        public string? Name { get; init; }
        public int Delay { get; init; }

        private sealed class Configure : IConfigureModel<CustomPreviewDirective>, IConfigureDirective<CustomPreviewDirective>
        {
            /// <inheritdoc/>
            public ValueTask ConfigureAsync(IModelBuilder<CustomPreviewDirective> builder, CancellationToken cancellationToken)
            {
                builder.Parameter(x => x.Name);

                builder.Option(x => x.Delay)
                    .IsRequired();

                return default;
            }

            /// <inheritdoc/>
            public ValueTask ConfigureAsync(IDirectiveBuilder<CustomPreviewDirective> builder, CancellationToken cancellationToken)
            {
                builder.AddAlias("cpreview")
                    .UseHandler<Handler>();

                return default;
            }
        }

        private sealed class Handler : IDirectiveHandler<CustomPreviewDirective>
        {
            private readonly IConsole _console;

            /// <summary>
            /// Initializes a new instance of <see cref="CustomPreviewDirective"/>.
            /// </summary>
            public Handler(IConsole console)
            {
                _console = console;
            }

            /// <inheritdoc/>
            public async ValueTask ExecuteAsync(DirectiveArgs<CustomPreviewDirective> args, StepDelegate next, CancellationToken cancellationToken = default)
            {
                CustomPreviewDirective model = args;
                _console.Output.WriteLine($"[cpreview {model.Name} --delay {model.Delay}] handled!");

                await next();
            }
        }
    }
}
