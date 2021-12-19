namespace Typin.Tests.Data.Invalid.CustomDirectives
{
    using System.Threading;
    using System.Threading.Tasks;

    public class NoAttributeDirective : IDirective
    {
        public ValueTask InitializeAsync(CancellationToken cancellationToken)
        {
            return default;
        }
    }
}
