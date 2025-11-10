using Markdig.Syntax.Inlines;

namespace UMarkdown.Parsing.Impl {
    public class EmphasisInlineRenderer : RichTextObjectRenderer<EmphasisInline> {
        protected override void Write(RichTextRenderer renderer, EmphasisInline obj) {
            var tag = GetDefaultTag(obj);
            if (tag == null) {
                renderer.WriteChildren(obj);
            } else {
                renderer.Write($"<{tag}>");
                renderer.WriteChildren(obj);
                renderer.Write($"</{tag}>");
            }
        }

        private static string GetDefaultTag(EmphasisInline obj) {
            return obj.DelimiterChar switch {
                '*' => obj.DelimiterCount == 2 ? "b" : "i",
                '_' => obj.DelimiterCount == 2 ? "u" : "i",
                '~' when obj.DelimiterCount == 2 => "s",
                '~' when obj.DelimiterCount == 1 => "sub",
                '^' when obj.DelimiterCount == 1 => "sup",
                _ => null
            };
        }
    }
}