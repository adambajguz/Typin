namespace Typin.Utilities.Diagnostics.Directives
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using PackSite.Library.Pipelining;
    using Typin;
    using Typin.Console;
    using Typin.Directives;
    using Typin.Directives.Builders;
    using Typin.Features.Input;
    using Typin.Models;
    using Typin.Models.Builders;

    /// <summary>
    /// When preview mode is specified (using the [preview] directive), the app will short-circuit by printing consumed command line arguments as they were parsed.
    /// This is useful when troubleshooting issues related to command routing and argument binding.
    /// </summary>
    public sealed class PreviewDirective : IDirective
    {
        private sealed class Configure : IConfigureModel<PreviewDirective>, IConfigureDirective<PreviewDirective>
        {
            /// <inheritdoc/>
            public ValueTask ConfigureAsync(IModelBuilder<PreviewDirective> builder, CancellationToken cancellationToken)
            {
                return default;
            }

            /// <inheritdoc/>
            public ValueTask ConfigureAsync(IDirectiveBuilder<PreviewDirective> builder, CancellationToken cancellationToken)
            {
                builder.Name(DiagnosticsDirectives.Preview)
                    .Description("The app will short-circuit by printing consumed command line arguments as they were parsed.")
                    .Handler<Handler>();

                return default;
            }
        }

        private sealed class Handler : IDirectiveHandler<PreviewDirective>
        {
            private readonly IConsole _console;

            /// <summary>
            /// Initializes a new instance of <see cref="PreviewDirective"/>.
            /// </summary>
            public Handler(IConsole console)
            {
                _console = console;
            }

            /// <inheritdoc/>
            public ValueTask ExecuteAsync(IDirectiveArgs<PreviewDirective> args, StepDelegate next, IInvokablePipeline<IDirectiveArgs> invokablePipeline, CancellationToken cancellationToken)
            {
                WriteCommandLineInput(_console, args.Context.Input.Parsed ?? throw new NullReferenceException("Input not set."));
                args.Context.Output.ExitCode ??= ExitCode.Success;

                return default;
            }

            private static void WriteCommandLineInput(IConsole console, ParsedInput input)
            {
                // Directives
                foreach (DirectiveInput directive in input.Directives)
                {
                    console.Output.WithForegroundColor(ConsoleColor.White, (output) => output.Write(directive.ToString()));

                    console.Output.Write(' ');
                }

                // Command name
                if (!string.IsNullOrWhiteSpace(input.CommandName))
                {
                    console.Output.WithForegroundColor(ConsoleColor.Cyan, (output) => output.Write(input.CommandName));

                    console.Output.Write(' ');
                }

                // Parameters
                foreach (ParameterInput parameter in input.Parameters)
                {
                    console.Output.Write('<');

                    console.Output.WithForegroundColor(ConsoleColor.White, (output) => output.Write(parameter));

                    console.Output.Write('>');
                    console.Output.Write(' ');
                }

                // Options
                foreach (OptionInput option in input.Options)
                {
                    console.Output.Write('(');

                    console.Output.WithForegroundColor(ConsoleColor.White, (output) =>
                    {
                        // Alias
                        output.Write(option.GetRawAlias());

                        // Values
                        if (option.Values.Any())
                        {
                            output.Write(' ');
                            output.Write(option.GetRawValues());
                        }
                    });

                    console.Output.Write(')');
                    console.Output.Write(' ');
                }

                console.Output.WriteLine();
            }
        }
    }
}