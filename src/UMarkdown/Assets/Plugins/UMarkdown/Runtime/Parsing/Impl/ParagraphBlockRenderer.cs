using Markdig.Syntax;

namespace UMarkdown.Parsing.Impl {
    public class ParagraphBlockRenderer : RichTextObjectRenderer<ParagraphBlock> {
        protected override void Write(RichTextRenderer renderer, ParagraphBlock obj) {
            if (!renderer.ImplicitParagraph) renderer.EnsureLine();

            renderer.WriteLeafInline(obj);
            if (!renderer.ImplicitParagraph) {
                renderer.EnsureLine();
                //renderer.WriteLine();
                renderer.VerticalSpace();
            }
        }
    }
}