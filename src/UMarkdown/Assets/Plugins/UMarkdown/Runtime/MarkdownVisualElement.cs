using UMarkdown.Parsing;
using UMarkdown.VisualElements;
using UnityEngine;
using UnityEngine.UIElements;
using DefaultMarkdownVisualElementFactory = UMarkdown.VisualElements.DefaultMarkdownVisualElementFactory;
using MarkdownVisualElementFactory = UMarkdown.VisualElements.MarkdownVisualElementFactory;
using RichTextMarkdownStyle = UMarkdown.Styling.RichTextMarkdownStyle;

namespace UMarkdown {
    /// <summary>
    ///     A UXML VisualElement that renders Markdown content.
    ///     It can render as a hierarchy of VisualElements (ExtendedMode = true)
    ///     or as a single Label with Unity rich text (ExtendedMode = false).
    /// </summary>
    [UxmlElement("MarkdownText")]
    public partial class MarkdownVisualElement : VisualElement {
        private MarkdownVisualElementFactory _elementFactory = new DefaultMarkdownVisualElementFactory();
        private bool _extendedMode = true;
        private string _markdownText = "";
        private RichTextMarkdownStyle _style = new();
        private bool _unescapeNewlines;

        /// <summary>
        ///     When true, literal "\n" sequences in the MarkdownText will be unescaped to real newlines
        ///     before rendering. Setting this property triggers a rebuild of the rendered content.
        /// </summary>
        [UxmlAttribute]
        public bool UnescapeNewlines {
            get => _unescapeNewlines;
            set {
                _unescapeNewlines = value;
                RebuildMarkdown();
            }
        }

        /// <summary>
        ///     The Markdown source text to render. Setting this property triggers a rebuild of the rendered content.
        /// </summary>
        [UxmlAttribute]
        [TextArea]
        public string MarkdownText {
            get => _markdownText;
            set {
                if (value == _markdownText) return;
                _markdownText = value;
                RebuildMarkdown();
            }
        }

        /// <summary>
        ///     If true, the markdown is converted into a tree of VisualElements using the configured ElementFactory.
        ///     If false, the markdown is converted to a single rich-text string and displayed in a Label.
        ///     Changing this property triggers a rebuild.
        /// </summary>
        [UxmlAttribute]
        public bool ExtendedMode {
            get => _extendedMode;
            set {
                _extendedMode = value;
                RebuildMarkdown();
            }
        }

        /// <summary>
        ///     Style object used when converting Markdown to rich-text or VisualElements.
        ///     Assigning a new style triggers a rebuild.
        /// </summary>
        [UxmlObjectReference("style")]
        public RichTextMarkdownStyle Style {
            get => _style;
            set {
                _style = value;
                RebuildMarkdown();
            }
        }

        /// <summary>
        ///     Factory used to create VisualElements for Markdown nodes when ExtendedMode is enabled.
        ///     Assigning a new factory triggers a rebuild.
        /// </summary>
        [UxmlObjectReference("elementFactory")]
        public MarkdownVisualElementFactory ElementFactory {
            get => _elementFactory;
            set {
                _elementFactory = value;
                RebuildMarkdown();
            }
        }

        /// <summary>
        ///     Clears any existing rendered children and rebuilds the rendered markdown content
        ///     according to the current MarkdownText, ExtendedMode, Style, and ElementFactory settings.
        /// </summary>
        public void RebuildMarkdown() {
            Clear();
            var normalized = _markdownText;
            if (_unescapeNewlines) normalized = normalized.Replace("\\n", "\n");
            if (_extendedMode) {
                var visualElement = UnityMarkdown.ToVisualElement(normalized, _style, _elementFactory);
                Add(visualElement);
            } else {
                var richText = UnityMarkdown.ToRichText(normalized, _style);
                var label = new Label(richText) {
                    enableRichText = true,
                    style = { whiteSpace = WhiteSpace.Normal }
                }.NoPaddingAndMargin();
                Add(label);
            }
        }
    }
}