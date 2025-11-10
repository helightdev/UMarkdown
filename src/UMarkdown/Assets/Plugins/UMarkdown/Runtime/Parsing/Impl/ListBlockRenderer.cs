using Markdig.Syntax;

namespace UMarkdown.Parsing.Impl {
    public class ListBlockRenderer : RichTextObjectRenderer<ListBlock> {
        protected override void Write(RichTextRenderer renderer, ListBlock obj) {
            var previousImplicit = renderer.ImplicitParagraph;
            renderer.ImplicitParagraph = true;
            if (!previousImplicit && renderer.Style.IndentRootLists) renderer.BeginIndent();

            foreach (var block in obj) {
                var item = (ListItemBlock)block;
                var prefix = obj.IsOrdered ? $"{item.Order}. " : "• ";
                renderer.BeginIndent(prefix);
                renderer.WriteChildren(item);
                renderer.EndIndent();
            }

            renderer.ImplicitParagraph = previousImplicit;
            if (!renderer.ImplicitParagraph) {
                if (renderer.Style.IndentRootLists) renderer.EndIndent();
                renderer.HintNewline();
                renderer.VerticalSpace();
            }
        }
    }
}