namespace Typin.Tests.Data.Invalid.CustomDirectives
{
    using Typin.Directives;
    using Typin.Directives.Attributes;

    [Directive("invalid-abstract", Description = "Abstract directive.")]
    public abstract class AbstractDirective : IDirective
    {

    }
}
