using System.Collections.Generic;
using UMarkdown.Parsing;
using UnityEngine.UIElements;

namespace UMarkdown.VisualElements {
    [UxmlObject]
    public abstract partial class MarkdownVisualElementFactory {
        public abstract VisualElement Compose(RichTextRenderer renderer, IReadOnlyList<VisualElement> blocks);
        public abstract VisualElement TextBlock(RichTextRenderer renderer, string richText);
        public abstract VisualElement CodeBlock(RichTextRenderer renderer, string richText);

        public abstract VisualElement IndentContainer(RichTextRenderer renderer, List<VisualElement> blocks,
            string prefix = null);

        public abstract VisualElement VerticalSpacer(RichTextRenderer renderer);
    }
}