namespace Typin.Tests.Data.Invalid.CustomDirectives
{
    using Typin.Directives;

    [Directive("invalid-abstract", Description = "Abstract directive.")]
    public abstract class AbstractDirective : IDirective
    {

    }
}
