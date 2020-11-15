namespace Typin.Directives
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
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
        public ValueTask HandleAsync(ICliContext context, CommandPipelineHandlerDelegate next, CancellationToken cancellationToken)
        {
            WriteCommandLineInput(context.Console, context.Input);
            context.ExitCode ??= ExitCodes.Success;

            return default;
        }

        private static void WriteCommandLineInput(IConsole console, CommandInput input)
        {
            // Directives
            console.Output.Write('{');
            foreach (DirectiveInput directive in input.Directives)
            {
                console.WithForegroundColor(ConsoleColor.White, () =>
                {
                    // Name
                    console.Output.Write(directive.Name.ToString());
                });

                if (directive != input.Directives[input.Directives.Count - 1])
                    console.Output.Write(' ');
            }
            console.Output.Write('}');
            console.Output.Write(' ');

            // Command name
            if (!string.IsNullOrWhiteSpace(input.CommandName))
            {
                console.WithForegroundColor(ConsoleColor.Cyan, () =>
                    console.Output.Write(input.CommandName));

                console.Output.Write(' ');
            }

            // Parameters
            foreach (CommandParameterInput parameter in input.Parameters)
            {
                console.Output.Write('<');

                console.WithForegroundColor(ConsoleColor.White, () =>
                    console.Output.Write(parameter));

                console.Output.Write('>');
                console.Output.Write(' ');
            }

            // Options
            foreach (CommandOptionInput option in input.Options)
            {
                console.Output.Write('[');

                console.WithForegroundColor(ConsoleColor.White, () =>
                {
                    // Alias
                    console.Output.Write(option.GetRawAlias());

                    // Values
                    if (option.Values.Any())
                    {
                        console.Output.Write(' ');
                        console.Output.Write(option.GetRawValues());
                    }
                });

                console.Output.Write(']');
                console.Output.Write(' ');
            }

            console.Output.WriteLine();
        }
    }
}
