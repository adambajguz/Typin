namespace TypinExamples.Shared.Components
{
    using Microsoft.AspNetCore.Components;
    using Microsoft.Extensions.Options;
    using TypinExamples.Configuration;
    using TypinExamples.Services;

    public partial class PageFooter : ComponentBase
    {
        [Inject] protected IMarkdownService Markdown { get; set; } = default!;

        [Inject] private IOptions<HeaderSettings> _HeaderSettings { get; set; } = default!;
        protected HeaderSettings HeaderSettings => _HeaderSettings.Value;
    }
}
