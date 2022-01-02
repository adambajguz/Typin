namespace Typin.Tests.Data.Invalid.CustomDirectives
{
    using System.Threading;
    using System.Threading.Tasks;
    using Typin.Directives.Attributes;

    [Directive("invalid-no-interface", Description = "No interface directive.")]
    public sealed class NoInterafaceDirective
    {
        public ValueTask OnInitializedAsync(CancellationToken cancellationToken)
        {
            return default;
        }
    }
}
