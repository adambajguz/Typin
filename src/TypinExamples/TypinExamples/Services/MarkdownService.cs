namespace TypinExamples.Services
{
    using Markdig;
    using Microsoft.AspNetCore.Components;
    using TypinExamples.Extensions;

    public interface IMarkdownService
    {
        MarkupString ToHtml(string content);
    }

    public class MarkdownService : IMarkdownService
    {
        private readonly MarkdownPipeline pipeline;

        public MarkdownService()
        {
            pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions()
                                                         .Use<TargetLinkExtension>()
                                                         .Build();
        }

        public MarkupString ToHtml(string content)
        {
            return (MarkupString)Markdown.ToHtml(content, pipeline);
        }
    }
}
