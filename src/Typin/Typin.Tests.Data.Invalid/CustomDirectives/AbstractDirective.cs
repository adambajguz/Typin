namespace Typin.Tests.Data.Invalid.CustomDirectives
{
    using System.Threading;
    using System.Threading.Tasks;
    using Typin;
    using Typin.Attributes;

    [Directive("invalid-abstract", Description = "Abstract directive.")]
    public abstract class AbstractDirective : IDirective
    {
        public ValueTask InitializeAsync(CancellationToken cancellationToken)
        {
            return default;
        }
    }
}
