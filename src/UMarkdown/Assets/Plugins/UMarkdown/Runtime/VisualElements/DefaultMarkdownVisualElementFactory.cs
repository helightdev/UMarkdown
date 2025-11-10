using System.Collections.Generic;
using UMarkdown.Parsing;
using UMarkdown.Styling;
using UnityEngine;
using UnityEngine.UIElements;

namespace UMarkdown.VisualElements {
    [UxmlObject]
    public partial class DefaultMarkdownVisualElementFactory : MarkdownVisualElementFactory {

        /// <summary>
        /// Vertical spacing (height in pixels) inserted between block elements.
        /// Default: 8f.
        /// </summary>
        [UxmlAttribute] public float VerticalSpacing { get; set; } = 8f;

        /// <summary>
        /// Background color used for code blocks. If left as default, code block background
        /// is derived from the code text style and style opacity.
        /// </summary>
        [UxmlAttribute] public Color CodeBlockColor { get; set; } = Color.black;

        /// <summary>
        /// Corner radius used for code block containers.
        /// Default: 8f.
        /// </summary>
        [UxmlAttribute] public float CodeBlockBorderRadius { get; set; } = 8f;

        /// <summary>
        /// Padding applied to code block containers. Format: (left, top, right, bottom).
        /// Default: new Vector4(8f, 4f, 8f, 4f).
        /// </summary>
        [UxmlAttribute] public Vector4 CodeBlockPadding { get; set; } = new(8f, 4f, 8f, 4f);

        /// <summary>
        /// Optional text style override used for code block text rendering.
        /// If null, the renderer's code text style is used.
        /// </summary>
        public TextStyle CodeBlockTextStyleOverride { get; set; }


        public override VisualElement Compose(RichTextRenderer renderer, IReadOnlyList<VisualElement> blocks) {
            var container = new VisualElement { name = "RichTextContainer" };
            foreach (var element in blocks) container.Add(element);

            return container;
        }

        public override VisualElement TextBlock(RichTextRenderer renderer, string richText) {
            var textStyle = renderer.TextStyleProvider.GetDefaultTextStyle(renderer);
            var label = new Label {
                text = richText,
                enableRichText = true,
                style = { whiteSpace = WhiteSpace.Normal },
                name = "TextContent"
            }.NoPaddingAndMargin();
            textStyle.ApplyTo(label);
            return label;
        }

        public override VisualElement CodeBlock(RichTextRenderer renderer, string richText) {
            var codeStyle = CodeBlockTextStyleOverride ?? renderer.TextStyleProvider.GetCodeTextStyle(renderer);
            var fontColor = codeStyle.Color ?? Color.black;
            fontColor.a *= renderer.Style.CodeBackgroundOpacity;
            if (CodeBlockColor != default) fontColor = CodeBlockColor;
            var label = new Label {
                text = richText,
                enableRichText = true,
                name = "CodeContent"
            }.NoPaddingAndMargin();
            codeStyle.ApplyTo(label);
            var container = new VisualElement {
                style = { backgroundColor = fontColor },
                name = "CodeBlock"
            }.Padding(CodeBlockPadding).BorderRadius(CodeBlockBorderRadius);
            if (codeStyle.Font) container.style.unityFontDefinition = FontDefinition.FromSDFFont(codeStyle.Font);

            container.Add(label);
            return container;
        }

        public override VisualElement IndentContainer(RichTextRenderer renderer, List<VisualElement> blocks,
            string prefix = null) {
            var container = new VisualElement {
                style = {
                    paddingLeft = renderer.Style.IntentSize,
                    flexShrink = 0
                },
                name = "IndentContainer"
            };
            if (prefix != null) {
                var prefixLabel = new Label {
                    text = prefix,
                    name = "IndentPrefix",
                    style = {
                        position = Position.Absolute,
                        width =  renderer.Style.IntentSize,
                        unityTextAlign = TextAnchor.UpperRight,
                        left = 0,
                        flexShrink = 0,
                        color = renderer.TextStyleProvider.GetDefaultTextStyle(renderer).Color ?? Color.white
                    }
                }.NoPaddingAndMargin();
                prefixLabel.style.paddingRight = 4;
                container.Add(prefixLabel);
            }

            foreach (var element in blocks) container.Add(element);

            return container;
        }

        public override VisualElement VerticalSpacer(RichTextRenderer renderer) {
            var spacer = new VisualElement {
                style = {
                    height = VerticalSpacing,
                    flexShrink = 0
                },
                name = "VerticalSpacer"
            };
            return spacer;
        }
    }

    public static class ElementExtensions {
        public static T NoPadding<T>(this T element) where T : VisualElement {
            element.style.paddingTop = 0;
            element.style.paddingLeft = 0;
            element.style.paddingRight = 0;
            element.style.paddingBottom = 0;
            return element;
        }

        public static T NoMargin<T>(this T element) where T : VisualElement {
            element.style.marginTop = 0;
            element.style.marginLeft = 0;
            element.style.marginRight = 0;
            element.style.marginBottom = 0;
            return element;
        }

        public static T NoBorder<T>(this T element) where T : VisualElement {
            element.style.borderTopWidth = 0;
            element.style.borderLeftWidth = 0;
            element.style.borderRightWidth = 0;
            element.style.borderBottomWidth = 0;
            return element;
        }

        public static T NoPaddingAndMargin<T>(this T element) where T : VisualElement {
            element.NoPadding();
            element.NoMargin();
            return element;
        }

        public static T Padding<T>(this T element, float value) where T : VisualElement {
            element.style.paddingTop = value;
            element.style.paddingLeft = value;
            element.style.paddingRight = value;
            element.style.paddingBottom = value;
            return element;
        }

        public static T Margin<T>(this T element, float value) where T : VisualElement {
            element.style.marginTop = value;
            element.style.marginLeft = value;
            element.style.marginRight = value;
            element.style.marginBottom = value;
            return element;
        }

        public static T BorderRadius<T>(this T element, float value) where T : VisualElement {
            element.style.borderTopLeftRadius = value;
            element.style.borderTopRightRadius = value;
            element.style.borderBottomLeftRadius = value;
            element.style.borderBottomRightRadius = value;
            return element;
        }

        public static T Padding<T>(this T element, Vector4 padding) where T : VisualElement {
            element.style.paddingLeft = padding.x;
            element.style.paddingTop = padding.y;
            element.style.paddingRight = padding.z;
            element.style.paddingBottom = padding.w;
            return element;
        }

        public static T Margin<T>(this T element, Vector4 margin) where T : VisualElement {
            element.style.marginLeft = margin.x;
            element.style.marginTop = margin.y;
            element.style.marginRight = margin.z;
            element.style.marginBottom = margin.w;
            return element;
        }

        public static Vector4 Padding<T>(this T element) where T : VisualElement {
            return new Vector4(
                element.style.paddingLeft.value.value,
                element.style.paddingTop.value.value,
                element.style.paddingRight.value.value,
                element.style.paddingBottom.value.value
            );
        }

        public static Vector4 Margin<T>(this T element) where T : VisualElement {
            return new Vector4(
                element.style.marginLeft.value.value,
                element.style.marginTop.value.value,
                element.style.marginRight.value.value,
                element.style.marginBottom.value.value
            );
        }
    }
}