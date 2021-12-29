namespace Typin.Modes.Interactive.Directives
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Options;
    using PackSite.Library.Pipelining;
    using Typin.Directives;
    using Typin.Directives.Attributes;
    using Typin.Features.Input;
    using Typin.Modes.Interactive;
    using Typin.Modes.Interactive.Internal.Extensions;

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
    [Directive(InteractiveOnlyDirectives.Scope, Description = "Sets a scope to command(s).")]
    public sealed class ScopeDirective : IDirective //TODO: add directive hadnler
    {
        private readonly InteractiveModeOptions _options;
        private readonly IRootSchemaAccessor _rootSchemaAccessor;

        /// <summary>
        /// Initializes an instance of <see cref="ScopeDirective"/>.
        /// </summary>
        public ScopeDirective(IOptions<InteractiveModeOptions> options, IRootSchemaAccessor rootSchemaAccessor)
        {
            _options = options.Value;
            _rootSchemaAccessor = rootSchemaAccessor;
        }

        /// <inheritdoc/>
        public ValueTask ExecuteAsync(CliContext args, StepDelegate next, IInvokablePipeline<CliContext> invokablePipeline, CancellationToken cancellationToken = default)
        {
            string? name = args.Input.Parsed?.CommandName ?? GetFallbackCommandName(args);

            if (name is not null)
            {
                _options.Scope = name;
                args.Output.ExitCode ??= ExitCode.Success;
            }

            return default;
        }

        private string? GetFallbackCommandName(CliContext context)
        {
            ParsedInput? input = context.Input.Parsed ?? throw new NullReferenceException("Input not set."); // maybe add some required check pattern/convention?

            string potentialName = context.Input.Arguments.Skip(input.Directives.Count)
                                                          .JoinToString(' ');

            if (_rootSchemaAccessor.RootSchema.IsCommandOrSubcommandPart(potentialName))
            {
                return potentialName;
            }

            return null;
        }
    }
}
