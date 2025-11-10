using Markdig.Syntax.Inlines;
using UnityEngine;

namespace UMarkdown.Parsing.Impl {
    public class CodeInlineRenderer : RichTextObjectRenderer<CodeInline> {
        protected override void Write(RichTextRenderer renderer, CodeInline obj) {
            var codeStyle = renderer.TextStyleProvider.GetCodeTextStyle(renderer);
            var fontColor = codeStyle.Color ?? Color.black;
            fontColor.a *= renderer.Style.CodeBackgroundOpacity;
            renderer.Write($"<mark=#{ColorUtility.ToHtmlStringRGBA(fontColor)}> ");
            renderer.Write(codeStyle.Begin());
            renderer.WriteEscaped(obj.Content);
            renderer.Write(codeStyle.End());
            renderer.Write(" </mark>");
        }
    }
}