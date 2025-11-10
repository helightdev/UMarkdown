using System.Text;
using UMarkdown.Parsing;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UIElements;

namespace UMarkdown.Styling {
    public class TextStyle {
        public FontAsset Font { get; set; }
        public float? FontSize { get; set; }
        public Color? Color { get; set; }
        public bool? Bold { get; set; }
        public bool? Italic { get; set; }

        public TextStyle Clone() {
            return new TextStyle {
                Font = Font,
                FontSize = FontSize,
                Color = Color,
                Bold = Bold,
                Italic = Italic
            };
        }

        public TextStyle Merge(TextStyle other) {
            return new TextStyle {
                Font = other.Font ?? Font,
                FontSize = other.FontSize ?? FontSize,
                Color = other.Color ?? Color,
                Bold = other.Bold ?? Bold,
                Italic = other.Italic ?? Italic
            };
        }

        public void ApplyTo(VisualElement element) {
            if (Font != null) element.style.unityFontDefinition = FontDefinition.FromSDFFont(Font);
            if (FontSize.HasValue) element.style.fontSize = FontSize.Value;
            if (Color.HasValue) element.style.color = Color.Value;
            var isBold = Bold.HasValue && Bold.Value;
            var isItalic = Italic.HasValue && Italic.Value;
            element.style.unityFontStyleAndWeight = isItalic switch {
                true when isBold => FontStyle.BoldAndItalic,
                true => FontStyle.Italic,
                _ => isBold ? FontStyle.Bold : FontStyle.Normal
            };
        }

        public string Begin() {
            var builder = new StringBuilder();
            Begin(builder);
            return builder.ToString();
        }

        public void Begin(StringBuilder builder) {
            if (Color.HasValue) {
                var hexColor = ColorUtility.ToHtmlStringRGBA(Color.Value);
                builder.Append($"<color=#{hexColor}>");
            }

            if (Bold.HasValue && Bold.Value) builder.Append("<b>");
            if (Italic.HasValue && Italic.Value) builder.Append("<i>");
            if (FontSize.HasValue) builder.Append($"<size={FontSize.Value}>");
            if (Font != null) builder.Append($"<font=\"{Font.name}\">");
        }

        public string End() {
            var builder = new StringBuilder();
            End(builder);
            return builder.ToString();
        }

        public void End(StringBuilder builder) {
            if (Font != null) builder.Append("</font>");
            if (FontSize.HasValue) builder.Append("</size>");
            if (Italic.HasValue && Italic.Value) builder.Append("</i>");
            if (Bold.HasValue && Bold.Value) builder.Append("</b>");
            if (Color.HasValue) builder.Append("</color>");
        }
    }

    [UxmlObject]
    public abstract partial class TextStyleProvider {
        public abstract TextStyle GetDefaultTextStyle(RichTextRenderer renderer);
        public abstract TextStyle GetCodeTextStyle(RichTextRenderer renderer);
        public abstract TextStyle GetHeaderTextStyle(RichTextRenderer renderer, int level);
    }

    [UxmlObject]
    public partial class DefaultTextStyleProvider : TextStyleProvider {
        public override TextStyle GetDefaultTextStyle(RichTextRenderer renderer) {
            return new TextStyle {
                FontSize = renderer.Style.FontSize,
                Color = renderer.Style.FontColor,
                Font = renderer.Style.TextFont
            };
        }

        public override TextStyle GetCodeTextStyle(RichTextRenderer renderer) {
            return GetDefaultTextStyle(renderer).Merge(new TextStyle {
                FontSize = renderer.Style.FontSize * 0.875f,
                Font = renderer.Style.CodeFont
            });
        }

        public override TextStyle GetHeaderTextStyle(RichTextRenderer renderer, int level) {
            var baseFontSize = renderer.Style.FontSize;
            var fontSize = level switch {
                1 => baseFontSize * 2.250f,
                2 => baseFontSize * 1.875f,
                3 => baseFontSize * 1.500f,
                4 => baseFontSize * 1.250f,
                5 => baseFontSize * 1.125f,
                _ => baseFontSize
            };
            return GetDefaultTextStyle(renderer).Merge(new TextStyle { FontSize = fontSize });
        }
    }
}