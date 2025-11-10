using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UIElements;

namespace UMarkdown.Styling {
    [UxmlObject]
    public partial class RichTextMarkdownStyle {
        /// <summary>
        /// When true, root-level lists are indented. When false, root lists align with other content.
        /// Default: true.
        /// </summary>
        [UxmlAttribute] public bool IndentRootLists { get; set; } = true;


        /// <summary>
        /// Primary font asset used for regular text.
        /// </summary>
        [UxmlAttribute] public FontAsset TextFont { get; set; }

        /// <summary>
        /// Font asset used for code/text inside code blocks.
        /// </summary>
        [UxmlAttribute] public FontAsset CodeFont { get; set; }

        /// <summary>
        /// Scale factor applied to indentation sizes.
        /// Default: 16f.
        /// </summary>
        [UxmlAttribute] public float IntentSize { get; set; } = 16f;

        /// <summary>
        /// Base font size (in points) for rendered text.
        /// Default: 14f.
        /// </summary>
        [UxmlAttribute] public float FontSize { get; set; } = 14f;

        /// <summary>
        /// Default color used for regular text when no explicit color is applied.
        /// Default: Color.white.
        /// </summary>
        [UxmlAttribute] public Color FontColor { get; set; } = Color.white;

        /// <summary>
        /// Opacity applied to the background color of code blocks (0..1).
        /// Default: 0.1f.
        /// </summary>
        [UxmlAttribute] public float CodeBackgroundOpacity { get; set; } = 0.1f;

        /// <summary>
        /// Reference to a TextStyleProvider instance used to obtain text and code text styles.
        /// Can be set via UXML object reference; defaults to DefaultTextStyleProvider.
        /// </summary>
        [UxmlObjectReference] public TextStyleProvider TextStyleProvider { get; set; } = new DefaultTextStyleProvider();
    }
}