using Markdig.Syntax.Inlines;

namespace UMarkdown.Parsing.Impl {
    public class LineBreakInlineRenderer : RichTextObjectRenderer<LineBreakInline> {
        protected override void Write(RichTextRenderer renderer, LineBreakInline obj) {
            if (renderer.IsLastInContainer) return;
            if (obj.IsHard) renderer.WriteLine();
            else renderer.Write(' ');
        }
    }
}