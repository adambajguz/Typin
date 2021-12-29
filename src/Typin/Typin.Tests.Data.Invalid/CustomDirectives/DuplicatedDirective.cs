namespace Typin.Tests.Data.Invalid.CustomDirectives
{
    using Typin.Attributes;
    using Typin.Directives;

    [Directive("preview", Description = "Duplicate directive.")]
    public class DuplicatedDirective : IDirective
    {

    }
}
