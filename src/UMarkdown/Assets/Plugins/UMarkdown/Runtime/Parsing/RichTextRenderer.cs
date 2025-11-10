using System.IO;
using Markdig.Renderers;
using Markdig.Syntax;
using UMarkdown.Parsing.Impl;

namespace UMarkdown.Parsing {
    public class RichTextRenderer : TextRendererBase<RichTextRenderer> {
        private readonly IRichTextConsumer _consumer;

        public RichTextRenderer(IRichTextConsumer consumer) : this(consumer, new Styling.RichTextMarkdownStyle()) { }

        public RichTextRenderer(IRichTextConsumer consumer, Styling.RichTextMarkdownStyle style) : base(new StringWriter()) {
            _consumer = consumer;

            ObjectRenderers.Add(new ParagraphBlockRenderer());
            ObjectRenderers.Add(new HeadingBlockRenderer());
            ObjectRenderers.Add(new ListBlockRenderer());
            ObjectRenderers.Add(new CodeBlockRenderer());
            ObjectRenderers.Add(new BlockQuoteRenderer());

            ObjectRenderers.Add(new EmphasisInlineRenderer());
            ObjectRenderers.Add(new LiteralInlineRenderer());
            ObjectRenderers.Add(new LineBreakInlineRenderer());
            ObjectRenderers.Add(new CodeInlineRenderer());
            ObjectRenderers.Add(new LinkInlineRenderer());

            ApplyStyle(style);
        }

        public bool ImplicitParagraph { get; set; } = false;
        public Styling.RichTextMarkdownStyle Style { get; private set; } = new();
        public Styling.TextStyleProvider TextStyleProvider => Style.TextStyleProvider;

        public void ApplyStyle(Styling.RichTextMarkdownStyle style) {
            Style = style;
        }

        private bool TryPopRichText(out string result) {
            Writer.Flush();
            result = Writer.ToString();
            Writer = new StringWriter();
            return !string.IsNullOrWhiteSpace(result);
        }

        public void PushAsTextBlock() {
            if (!TryPopRichText(out var str)) return;
            _consumer.AcceptTextBlock(this, str);
        }

        public void PushAsCodeBlock() {
            TryPopRichText(out var str);
            _consumer.AcceptCodeBlock(this, str);
        }

        public void BeginIndent(string prefix = null) {
            PushAsTextBlock();
            _consumer.BeginIndent(this, prefix);
        }

        public void EndIndent() {
            PushAsTextBlock();
            _consumer.EndIndent(this);
        }

        public void VerticalSpace() {
            PushAsTextBlock();
            _consumer.EnsureVerticalSpace(this);
        }

        public void HintNewline() {
            _consumer.HintNewline(this);
        }

        public void WriteEscaped(string text) {
            Write(text.Replace("<", "<<space=0>"));
        }

        public override object Render(MarkdownObject markdownObject) {
            base.Render(markdownObject);
            PushAsTextBlock();
            return _consumer.Finalize(this);
        }
    }

    public abstract class RichTextObjectRenderer<TObject> : MarkdownObjectRenderer<RichTextRenderer, TObject>
        where TObject : MarkdownObject { }

    public interface IRichTextConsumer {
        public void AcceptCodeBlock(RichTextRenderer renderer, string richText);
        public void AcceptTextBlock(RichTextRenderer renderer, string richText);
        public void BeginIndent(RichTextRenderer renderer, string prefix = null);
        public void EndIndent(RichTextRenderer renderer);
        public void EnsureVerticalSpace(RichTextRenderer renderer);
        public void HintNewline(RichTextRenderer renderer) { }
        public object Finalize(RichTextRenderer renderer);
    }
}