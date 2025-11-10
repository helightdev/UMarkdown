using System;
using System.Text;
using ColorCode;
using ColorCode.Styling;
using Markdig.Syntax;
using UMarkdown.Highlight;

namespace UMarkdown.Parsing.Impl {
    public class CodeBlockRenderer : RichTextObjectRenderer<CodeBlock> {
        protected override void Write(RichTextRenderer renderer, CodeBlock obj) {
            var exportAsSection = false;
            if (!renderer.ImplicitParagraph) {
                renderer.PushAsTextBlock();
                exportAsSection = true;
            }

            var buffer = new StringBuilder();
            foreach (var line in obj.Lines.Lines) {
                buffer.Append(line.Slice.AsSpan());
                buffer.AppendLine();
            }

            if (obj is FencedCodeBlock { Info: { Length: > 0 } } fenced) {
                try {
                    var language = fenced.Info!;
                    var formatter = new RichTextColorizerFormatter(StyleDictionary.DefaultLight);
                    var langInstance = Languages.FindById(language);
                    if (langInstance == null) goto Default;
                    formatter.Format(buffer.ToString(), langInstance);
                    renderer.EnsureLine();
                    renderer.WriteLine(formatter.GetString());
                    renderer.EnsureLine();
                    goto End;
                } catch (Exception e) {
                    // ignored
                }
            }

            Default:
            {
                renderer.EnsureLine();
                renderer.WriteEscaped(buffer.ToString());
                renderer.EnsureLine();
            }
            End:
            {
                if (exportAsSection) {
                    renderer.PushAsCodeBlock();
                }
            }
        }
    }
}