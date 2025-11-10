using Markdig.Syntax;

namespace UMarkdown.Parsing.Impl {
    public class HeadingBlockRenderer : RichTextObjectRenderer<HeadingBlock> {
        protected override void Write(RichTextRenderer renderer, HeadingBlock obj) {
            renderer.EnsureLine();
            var textStyle = renderer.TextStyleProvider.GetHeaderTextStyle(renderer, obj.Level);
            renderer.Write(textStyle.Begin());
            renderer.WriteLeafInline(obj);
            renderer.Write(textStyle.End());
            renderer.EnsureLine();
            renderer.VerticalSpace();
        }
    }
}