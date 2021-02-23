namespace TypinExamples.Extensions
{
    using System;
    using Markdig;
    using Markdig.Renderers;
    using Markdig.Renderers.Html;
    using Markdig.Renderers.Html.Inlines;
    using Markdig.Syntax;
    using Markdig.Syntax.Inlines;

    public class TargetLinkExtension : IMarkdownExtension
    {
        public void Setup(MarkdownPipelineBuilder pipeline)
        {

        }

        public void Setup(MarkdownPipeline pipeline, IMarkdownRenderer renderer)
        {
            if (renderer is HtmlRenderer htmlRenderer)
            {
                var linkInlineRenderer = htmlRenderer.ObjectRenderers.FindExact<LinkInlineRenderer>();
                if (linkInlineRenderer != null)
                {
                    linkInlineRenderer.TryWriters.Add(TryLinkInlineRenderer);
                }

                var autoLinkInlineRenderer = htmlRenderer.ObjectRenderers.FindExact<AutolinkInlineRenderer>();
                if (autoLinkInlineRenderer != null)
                {
                    autoLinkInlineRenderer.TryWriters.Add(TryAutoLinkInlineRenderer);
                }
            }
        }

        private bool TryLinkInlineRenderer(HtmlRenderer renderer, LinkInline linkInline)
        {
            TryAddTarget(linkInline.Url, linkInline);
            return false;
        }
        private bool TryAutoLinkInlineRenderer(HtmlRenderer renderer, AutolinkInline autolinkInline)
        {
            TryAddTarget(autolinkInline.Url, autolinkInline);
            return false;
        }

        private void TryAddTarget(string url, MarkdownObject obj)
        {
            if (url != null && Uri.TryCreate(url, UriKind.Absolute, out _))
            {
                obj.GetAttributes().AddPropertyIfNotExist("target", "_blank");
            }
        }
    }
}
