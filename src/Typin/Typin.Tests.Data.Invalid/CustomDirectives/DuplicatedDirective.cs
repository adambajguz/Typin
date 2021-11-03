namespace Typin.Tests.Data.CustomDirectives.Invalid
{
    using System.Threading;
    using System.Threading.Tasks;
    using Typin;
    using Typin.Attributes;

    [Directive("preview", Description = "Duplicate directive.")]
    public class DuplicatedDirective : IDirective
    {
        public ValueTask InitializeAsync(CancellationToken cancellationToken)
        {
            return default;
        }
    }
}
