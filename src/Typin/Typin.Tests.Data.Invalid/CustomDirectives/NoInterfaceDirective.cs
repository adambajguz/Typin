namespace Typin.Tests.Data.CustomDirectives.Invalid
{
    using System.Threading;
    using System.Threading.Tasks;
    using Typin.Attributes;

    [Directive("invalid-no-interface", Description = "No interface directive.")]
    public sealed class NoInterafaceDirective
    {
        public ValueTask OnInitializedAsync(CancellationToken cancellationToken)
        {
            return default;
        }
    }
}
