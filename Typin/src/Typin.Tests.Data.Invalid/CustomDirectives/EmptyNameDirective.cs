namespace Typin.Tests.Data.Invalid.CustomDirectives
{
    using Typin.Directives;
    using Typin.Directives.Attributes;

    [Directive("  ", Description = "Empty name directive.")]
    public class EmptyNameDirective : IDirective
    {

    }
}
