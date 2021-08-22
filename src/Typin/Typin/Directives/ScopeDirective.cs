namespace Typin.Directives
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Options;
    using PackSite.Library.Pipelining;
    using Typin.Attributes;
    using Typin.Input;
    using Typin.Internal.Extensions;
    using Typin.Modes;

    /// <summary>
    /// If application runs in interactive mode, [>] directive followed by command(s) would scope to the command(s), allowing to ommit specified command name(s).
    /// <example>
    /// Commands:
    ///              > [>] cmd1 sub
    ///      cmd1 sub> list
    ///      cmd1 sub> get
    ///              > [>] cmd1
    ///          cmd1> test
    ///          cmd1> -h
    ///
    /// are an equivalent to:
    ///              > cmd1 sub list
    ///              > cmd1 sub get
    ///              > cmd1 test
    ///              > cmd1 -h
    /// </example>
    /// </summary>
    [Directive(BuiltInDirectives.Scope, Description = "Sets a scope to command(s).", SupportedModes = new[] { typeof(InteractiveMode) })]
    public sealed class ScopeDirective : IPipelinedDirective
    {
        private readonly InteractiveModeOptions _options;

        /// <summary>
        /// Initializes an instance of <see cref="ScopeDirective"/>.
        /// </summary>
        public ScopeDirective(IOptions<InteractiveModeOptions> options)
        {
            _options = options.Value;
        }

        /// <inheritdoc/>
        public ValueTask OnInitializedAsync(CancellationToken cancellationToken)
        {
            return default;
        }

        /// <inheritdoc/>
        public ValueTask ExecuteAsync(ICliContext args, StepDelegate next, IInvokablePipeline<ICliContext> invokablePipeline, CancellationToken cancellationToken = default)
        {
            string? name = args.Input.CommandName ?? GetFallbackCommandName(args);

            if (name is not null)
            {
                _options.Scope = name;
                args.ExitCode ??= ExitCodes.Success;
            }

            return default;
        }

        private static string? GetFallbackCommandName(ICliContext context)
        {
            CommandInput input = context.Input;

            string potentialName = input.Arguments.Skip(input.Directives.Count)
                                                  .JoinToString(' ');

            if (context.RootSchema.IsCommandOrSubcommandPart(potentialName))
            {
                return potentialName;
            }

            return null;
        }
    }
}
