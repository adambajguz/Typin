namespace Typin.Tests.Data.CustomDirectives.Invalid
{
    using System.Threading;
    using System.Threading.Tasks;
    using Typin;
    using Typin.Attributes;

    [Directive("  ", Description = "Empty name directive.")]
    public class EmptyNameDirective : IDirective
    {
        public ValueTask OnInitializedAsync(CancellationToken cancellationToken)
        {
            return default;
        }
    }
}
