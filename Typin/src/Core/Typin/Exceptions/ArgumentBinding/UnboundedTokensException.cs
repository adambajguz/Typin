namespace Typin.Exceptions.ArgumentBinding
{
    using System.Collections.Generic;
    using System.Text;
    using Typin.Features.Binding;

    /// <summary>
    /// Unbounded tokens exception.
    /// </summary>
    public sealed class UnboundedTokensException : ArgumentBindingException
    {
        /// <summary>
        /// Initializes an instance of <see cref="UnboundedTokensException"/>.
        /// </summary>
        public UnboundedTokensException(IUnboundedDirectiveCollection unboundedDirectiveCollection) :
            base(null,
                 BuildMessage(unboundedDirectiveCollection))
        {

        }

        private static string BuildMessage(IUnboundedDirectiveCollection unboundedDirectiveCollection)
        {
            StringBuilder builder = new();
            builder.AppendLine("One or more tokens were not bounded:");

            foreach (IUnboundedDirectiveToken unboundedDirective in unboundedDirectiveCollection)
            {
                if (unboundedDirective.HasUnbounded)
                {
                    builder.Append(' ', 2);
                    builder.Append('[');
                    builder.Append(unboundedDirective.Alias);
                    builder.Append(": ");

                    IList<string> children = unboundedDirective.Children.GetRaw();
                    builder.Append('<');
                    builder.AppendJoin("> <", children);
                    builder.AppendLine(">]");
                }
            }

            return builder.ToString();
        }
    }
}