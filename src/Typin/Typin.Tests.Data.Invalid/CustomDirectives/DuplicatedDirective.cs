namespace Typin.Tests.Data.Invalid.CustomDirectives
{
    using Typin.Directives;
    using Typin.Directives.Attributes;

    [Directive("preview", Description = "Duplicate directive.")]
    public class DuplicatedDirective : IDirective
    {

    }
}
