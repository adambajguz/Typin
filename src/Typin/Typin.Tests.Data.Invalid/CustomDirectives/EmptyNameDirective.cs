namespace Typin.Tests.Data.Invalid.CustomDirectives
{
    using System.Threading;
    using System.Threading.Tasks;
    using Typin.Attributes;

    [Directive("  ", Description = "Empty name directive.")]
    public class EmptyNameDirective : IDirective
    {
        public ValueTask InitializeAsync(CancellationToken cancellationToken)
        {
            return default;
        }
    }
}
