using Markdig;
using Markdig.Syntax;
using UMarkdown.Parsing;
using UMarkdown.String;
using UMarkdown.VisualElements;
using UnityEngine.UIElements;
using RichTextMarkdownStyle = UMarkdown.Styling.RichTextMarkdownStyle;

namespace UMarkdown {
    /// <summary>
    ///     Utility methods for parsing Markdown and converting it to Unity-friendly formats
    ///     such as rich-text strings or VisualElement trees.
    /// </summary>
    public static class UnityMarkdown {
        /// <summary>
        ///     Parses the provided Markdown string into a Markdig <see cref="MarkdownDocument" />
        ///     using a pipeline configured with common extensions (e.g. emphasis extras).
        /// </summary>
        /// <param name="markdown">The Markdown source to parse.</param>
        /// <returns>A parsed <see cref="MarkdownDocument" />.</returns>
        public static MarkdownDocument ParseDocument(string markdown) {
            return Markdown.Parse(markdown, new MarkdownPipelineBuilder()
                .UseEmphasisExtras()
                .Build());
        }

        /// <summary>
        ///     Converts the given Markdown to a Unity rich-text formatted string.
        /// </summary>
        /// <param name="markdown">The Markdown source to convert.</param>
        /// <param name="style">
        ///     Optional style used by the renderer. If null a default <see cref="RichTextMarkdownStyle" /> is
        ///     used.
        /// </param>
        /// <returns>A string containing Unity rich-text representing the rendered Markdown.</returns>
        public static string ToRichText(string markdown, RichTextMarkdownStyle style = null) {
            style ??= new RichTextMarkdownStyle();
            var document = ParseDocument(markdown);
            var consumer = new RichTextStringAccumulator();
            var renderer = new RichTextRenderer(consumer, style);
            var str = renderer.Render(document) as string;
            return str;
        }

        /// <summary>
        ///     Converts the given Markdown to a <see cref="VisualElement" /> tree.
        /// </summary>
        /// <param name="markdown">The Markdown source to convert.</param>
        /// <param name="style">
        ///     Optional style used by the renderer. If null a default <see cref="RichTextMarkdownStyle" /> is
        ///     used.
        /// </param>
        /// <param name="factory">
        ///     Optional factory for creating VisualElements. If null
        ///     <see cref="DefaultMarkdownVisualElementFactory" /> is used.
        /// </param>
        /// <returns>A <see cref="VisualElement" /> that contains the rendered Markdown UI.</returns>
        public static VisualElement ToVisualElement(string markdown, RichTextMarkdownStyle style = null,
            MarkdownVisualElementFactory factory = null) {
            style ??= new RichTextMarkdownStyle();
            factory ??= new DefaultMarkdownVisualElementFactory();
            var document = ParseDocument(markdown);
            var consumer = new VisualElementRichTextConsumer(factory);
            var renderer = new RichTextRenderer(consumer, style);
            var element = renderer.Render(document) as VisualElement;
            return element;
        }
    }
}