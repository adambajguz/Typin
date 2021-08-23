﻿namespace Typin.Directives
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using PackSite.Library.Pipelining;
    using Typin.Attributes;
    using Typin.Console;
    using Typin.Input;

    /// <summary>
    /// When preview mode is specified (using the [preview] directive), the app will short-circuit by printing consumed command line arguments as they were parsed.
    /// This is useful when troubleshooting issues related to command routing and argument binding.
    /// </summary>
    [Directive(BuiltInDirectives.Preview, Description = "The app will short-circuit by printing consumed command line arguments as they were parsed.")]
    public sealed class PreviewDirective : IPipelinedDirective
    {
        /// <inheritdoc/>
        public ValueTask OnInitializedAsync(CancellationToken cancellationToken)
        {
            return default;
        }

        /// <inheritdoc/>
        public ValueTask ExecuteAsync(ICliContext args, StepDelegate next, IInvokablePipeline<ICliContext> invokablePipeline, CancellationToken cancellationToken = default)
        {
            WriteCommandLineInput(args.Console, args.Input);
            args.ExitCode ??= ExitCodes.Success;

            return default;
        }

        private static void WriteCommandLineInput(IConsole console, CommandInput input)
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
            foreach (CommandParameterInput parameter in input.Parameters)
            {
                console.Output.Write('<');

                console.Output.WithForegroundColor(ConsoleColor.White, (output) => output.Write(parameter));

                console.Output.Write('>');
                console.Output.Write(' ');
            }

            // Options
            foreach (CommandOptionInput option in input.Options)
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
