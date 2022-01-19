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

    public sealed class CustomHelpDirective : IDirective
    {
        private sealed class Configure : IConfigureModel<CustomHelpDirective>, IConfigureDirective<CustomHelpDirective>
        {
            /// <inheritdoc/>
            public ValueTask ConfigureAsync(IModelBuilder<CustomHelpDirective> builder, CancellationToken cancellationToken)
            {
                return default;
            }

            /// <inheritdoc/>
            public ValueTask ConfigureAsync(IDirectiveBuilder<CustomHelpDirective> builder, CancellationToken cancellationToken)
            {
                builder.AddAlias("chelp")
                    .UseHandler<Handler>();

                return default;
            }
        }

        private sealed class Handler : IDirectiveHandler<CustomHelpDirective>
        {
            private readonly IConsole _console;

            /// <summary>
            /// Initializes a new instance of <see cref="CustomHelpDirective"/>.
            /// </summary>
            public Handler(IConsole console)
            {
                _console = console;
            }

            /// <inheritdoc/>
            public async ValueTask ExecuteAsync(IDirectiveArgs<CustomHelpDirective> args, StepDelegate next, IInvokablePipeline<IDirectiveArgs> invokablePipeline, CancellationToken cancellationToken)
            {
                _console.Output.WriteLine("[chelp] handled!");

                await next();
            }
        }
    }
}
