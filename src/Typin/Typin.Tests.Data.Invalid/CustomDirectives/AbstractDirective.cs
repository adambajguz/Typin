namespace Typin.Tests.Data.Invalid.CustomDirectives
{
    using System.Threading;
    using System.Threading.Tasks;
    using Typin.Attributes;
    using Typin.Directives;

    [Directive("invalid-abstract", Description = "Abstract directive.")]
    public abstract class AbstractDirective : IDirective
    {
        public ValueTask InitializeAsync(CancellationToken cancellationToken)
        {
            return default;
        }
    }
}
