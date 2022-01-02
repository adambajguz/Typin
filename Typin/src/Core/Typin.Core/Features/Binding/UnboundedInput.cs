namespace Typin.Features.Binding
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Text;
    using Typin.Features.Input;
    using Typin.Internal.Extensions;

    /// <summary>
    /// Contains input that is pending to be bounded.
    /// </summary>
    public sealed class UnboundedInput
    {
        /// <summary>
        /// Command direcitves list without special [interactive] directive.
        /// </summary>
        public List<DirectiveInput> Directives { get; set; }

        /// <summary>
        /// Command parameters list.
        /// </summary>
        public List<ParameterInput> Parameters { get; set; }

        /// <summary>
        /// Command options list.
        /// </summary>
        public List<OptionInput> Options { get; set; }

        /// <summary>
        /// Initializes an instance of <see cref="UnboundedInput"/>.
        /// </summary>
        public UnboundedInput(ParsedInput parsedInput)
        {
            Directives = parsedInput.Directives.ToList();
            Parameters = parsedInput.Parameters.ToList();
            Options = parsedInput.Options.ToList();
        }

        /// <summary>
        /// Whether command has a directive.
        /// </summary>
        public bool HasDirective(string directive)
        {
            string v = directive.Trim('[', ']');

            return Directives.Where(x => x.Name == v)
                             .Any();
        }

        /// <inheritdoc/>
        [ExcludeFromCodeCoverage]
        public override string ToString()
        {
            StringBuilder buffer = new();

            foreach (DirectiveInput directive in Directives)
            {
                buffer.AppendIfNotEmpty(' ')
                      .Append(directive);
            }

            foreach (ParameterInput parameter in Parameters)
            {
                buffer.AppendIfNotEmpty(' ')
                      .Append(parameter);
            }

            foreach (OptionInput option in Options)
            {
                buffer.AppendIfNotEmpty(' ')
                      .Append(option);
            }

            return buffer.ToString();
        }
    }
}