using Markdig.Syntax.Inlines;

namespace UMarkdown.Parsing.Impl {
    public class LiteralInlineRenderer : RichTextObjectRenderer<LiteralInline> {
        protected override void Write(RichTextRenderer renderer, LiteralInline obj) {
            renderer.Write(ref obj.Content);
        }
    }
}