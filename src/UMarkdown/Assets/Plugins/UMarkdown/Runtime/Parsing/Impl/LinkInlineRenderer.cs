using Markdig.Syntax.Inlines;

namespace UMarkdown.Parsing.Impl {
    public class LinkInlineRenderer : RichTextObjectRenderer<LinkInline> {
        protected override void Write(RichTextRenderer renderer, LinkInline obj) {
            renderer.Write("<a href=\"");
            renderer.Write(obj.GetDynamicUrl != null ? obj.GetDynamicUrl() : obj.Url);
            renderer.Write("\">");
            renderer.WriteChildren(obj);
            renderer.Write("</a>");
        }
    }
}