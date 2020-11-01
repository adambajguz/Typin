namespace TypinExamples.Shared.Components
{
    using Microsoft.AspNetCore.Components;
    using Microsoft.Extensions.Options;
    using TypinExamples.Configuration;
    using TypinExamples.Services;

    public partial class PageHeader : ComponentBase
    {
        [Inject] private IMarkdownService Markdown { get; set; } = default!;

        [Inject] private IOptions<ApplicationSettings> _AppSettings { get; set; } = default!;
        private ApplicationSettings AppSettings => _AppSettings.Value;

        [Inject] private IOptions<HeaderSettings> _HeaderSettings { get; set; } = default!;
        private HeaderSettings HeaderSettings => _HeaderSettings.Value;
    }
}
