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
                builder.Name("cpreview")
                    .Handler<Handler>();

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
            public async ValueTask ExecuteAsync(IDirectiveArgs<CustomPreviewDirective> args, StepDelegate next, IInvokablePipeline<IDirectiveArgs> invokablePipeline, CancellationToken cancellationToken)
            {
                _console.Output.WriteLine($"[cpreview {args.Directive.Name} --delay {args.Directive.Delay}] handled!");

                await next();
            }
        }
    }
}
