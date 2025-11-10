using Markdig.Syntax;

namespace UMarkdown.Parsing.Impl {
    public class BlockQuoteRenderer : RichTextObjectRenderer<QuoteBlock> {
        protected override void Write(RichTextRenderer renderer, QuoteBlock obj) {
            renderer.BeginIndent();
            renderer.WriteChildren(obj);
            renderer.EndIndent();
            renderer.EnsureLine();
        }
    }
}