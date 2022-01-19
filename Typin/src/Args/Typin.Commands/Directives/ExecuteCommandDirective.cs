namespace Typin.Commands.Directives
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using PackSite.Library.Pipelining;
    using Typin.Directives;
    using Typin.Directives.Builders;
    using Typin.Models;
    using Typin.Models.Builders;

    /// <summary>
    /// A directive that executes a command.
    /// </summary>
    public sealed class ExecuteCommandDirective : IDirective
    {
        /// <summary>
        /// <see cref="ExecuteCommandDirective"/> handler.
        /// </summary>
        public sealed class Handler : IDirectiveHandler<ExecuteCommandDirective>
        {
            /// <inheritdoc/>
            public ValueTask ExecuteAsync(IDirectiveArgs<ExecuteCommandDirective> args, StepDelegate next, IInvokablePipeline<IDirectiveArgs> invokablePipeline, CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Configures <see cref="ExecuteCommandDirective"/>.
        /// </summary>
        public sealed class Configure : IConfigureModel<ExecuteCommandDirective>, IConfigureDirective<ExecuteCommandDirective>
        {
            /// <inheritdoc/>
            public ValueTask ConfigureAsync(IModelBuilder<ExecuteCommandDirective> builder, CancellationToken cancellationToken)
            {
                return default;
            }

            /// <inheritdoc/>
            public ValueTask ConfigureAsync(IDirectiveBuilder<ExecuteCommandDirective> builder, CancellationToken cancellationToken)
            {
                builder
                    .ManageAliases(aliases =>
                    {
                        aliases.Add(string.Empty);
                        aliases.Add("exec");
                    })
                    .UseHandler<Handler>();

                return default;
            }
        }
    }
}
