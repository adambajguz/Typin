namespace Typin.Pipeline
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using PackSite.Library.Pipelining;
    using Typin;
    using Typin.Input;
    using Typin.Internal.Input;

    /// <summary>
    /// Resolves input.
    /// </summary>
    public sealed class ResolveInput : IMiddleware
    {
        private readonly IRootSchemaAccessor _rootSchemaAccessor;

        /// <summary>
        /// Initializes a new instance of <see cref="BindInput"/>.
        /// </summary>
        public ResolveInput(IRootSchemaAccessor rootSchemaAccessor)
        {
            _rootSchemaAccessor = rootSchemaAccessor;
        }

        /// <inheritdoc/>
        public async ValueTask ExecuteAsync(CliContext args, StepDelegate next, IInvokablePipeline<CliContext> invokablePipeline, CancellationToken cancellationToken = default)
        {
            bool trimExecutable = args.Input.ExecutionOptions.HasFlag(CommandExecutionOptions.TrimExecutable);

            ParsedCommandInput input = InputResolver.Parse(
                args.Input.Arguments.Skip(trimExecutable ? 1 : 0),
                _rootSchemaAccessor.RootSchema.GetCommandNames());

            args.Input.Parsed = input;

            await next();
        }
    }
}
