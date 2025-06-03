namespace Typin.Tests.Data.Invalid.CustomDirectives
{
    using Typin.Directives;

    [Directive("  ", Description = "Empty name directive.")]
    public class EmptyNameDirective : IDirective
    {

    }
}
