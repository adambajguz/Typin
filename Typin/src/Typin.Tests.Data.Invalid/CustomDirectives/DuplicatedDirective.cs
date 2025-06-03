namespace Typin.Tests.Data.Invalid.CustomDirectives
{
    using Typin.Directives;

    [Directive("preview", Description = "Duplicate directive.")]
    public class DuplicatedDirective : IDirective
    {

    }
}
