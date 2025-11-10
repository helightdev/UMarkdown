using System.Collections.Generic;
using UMarkdown.Parsing;
using UnityEngine.UIElements;

namespace UMarkdown.VisualElements {
    public class VisualElementRichTextConsumer : IRichTextConsumer {
        private readonly MarkdownVisualElementFactory _factory;
        private readonly Stack<BlockFrame> _frames = new();

        public VisualElementRichTextConsumer(MarkdownVisualElementFactory factory) {
            _factory = factory;
            _frames.Push(new ComposeBlockFrame());
        }

        private BlockFrame CurrentFrame => _frames.Peek();

        public void AcceptCodeBlock(RichTextRenderer renderer, string richText) {
            var element = _factory.CodeBlock(renderer, richText);
            CurrentFrame.AddBlock(element);
        }

        public void AcceptTextBlock(RichTextRenderer renderer, string richText) {
            var element = _factory.TextBlock(renderer, richText.Trim());
            CurrentFrame.AddBlock(element);
        }

        public void BeginIndent(RichTextRenderer renderer, string prefix = null) {
            _frames.Push(new IndentBlockFrame { Prefix = prefix });
        }

        public void EndIndent(RichTextRenderer renderer) {
            CloseFrame<IndentBlockFrame>(renderer);
        }

        public void EnsureVerticalSpace(RichTextRenderer renderer) {
            var element = _factory.VerticalSpacer(renderer);
            CurrentFrame.AddBlock(element);
        }

        public object Finalize(RichTextRenderer renderer) {
            // Close all remaining frames
            while (_frames.Count > 1) CloseFrame<BlockFrame>(renderer);

            return _factory.Compose(renderer, CurrentFrame.Blocks);
        }

        public bool CloseFrame<T>(RichTextRenderer renderer) {
            if (CurrentFrame is not T || _frames.Count <= 1) return false;
            var frame = _frames.Pop();
            var element = frame.CloseFrame(renderer, _factory);
            CurrentFrame.AddBlock(element);
            return true;
        }
    }

    public abstract class BlockFrame {
        public List<VisualElement> Blocks { get; } = new();

        public void AddBlock(VisualElement element) {
            Blocks.Add(element);
        }

        public abstract VisualElement CloseFrame(RichTextRenderer renderer, MarkdownVisualElementFactory factory);
    }

    public class ComposeBlockFrame : BlockFrame {
        public override VisualElement CloseFrame(RichTextRenderer renderer, MarkdownVisualElementFactory factory) {
            return factory.Compose(renderer, Blocks);
        }
    }

    public class IndentBlockFrame : BlockFrame {
        public string Prefix { get; set; }

        public override VisualElement CloseFrame(RichTextRenderer renderer, MarkdownVisualElementFactory factory) {
            return factory.IndentContainer(renderer, Blocks, Prefix);
        }
    }
}