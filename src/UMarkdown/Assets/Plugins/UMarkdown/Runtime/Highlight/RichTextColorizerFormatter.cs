using System.Collections.Generic;
using System.Text;
using ColorCode;
using ColorCode.Common;
using ColorCode.Parsing;
using ColorCode.Styling;

namespace UMarkdown.Highlight {
    public class RichTextColorizerFormatter : CodeColorizerBase {
        private readonly StringBuilder _builder = new();

        public RichTextColorizerFormatter(StyleDictionary style = null, ILanguageParser languageParser = null) : base(
            style, languageParser) { }

        public void Format(string sourceCode, ILanguage language) {
            languageParser.Parse(sourceCode, language, Write);
        }

        public string GetString() {
            return _builder.ToString();
        }

        protected override void Write(string parsedSourceCode, IList<Scope> scopes) {
            var styleInsertions = new List<TextInsertion>();

            foreach (var scope in scopes)
                GetStyleInsertionsForCapturedStyle(scope, styleInsertions);

            styleInsertions.SortStable((x, y) => x.Index.CompareTo(y.Index));

            var offset = 0;

            Scope previousScope = null;
            foreach (var insertion in styleInsertions) {
                var text = parsedSourceCode.Substring(offset, insertion.Index - offset);
                AppendSpan(text, previousScope);
                if (!string.IsNullOrWhiteSpace(insertion.Text)) AppendSpan(text, previousScope);

                offset = insertion.Index;

                previousScope = insertion.Scope;
            }

            var remaining = parsedSourceCode.Substring(offset);
            // Ensures that those loose carriages don't run away!
            if (remaining != "\r") AppendSpan(remaining, null);
        }

        private void AppendSpan(string text, Scope scope) {
            text = text.Replace("<", "<<space=0>");

            if (scope == null || !Styles.TryGetValue(scope.Name, out var style)) {
                _builder.Append(text);
                return;
            }

            var hasForeground = !string.IsNullOrWhiteSpace(style.Foreground);
            if (hasForeground) {
                // ColorCode uses #ARGB, so we need to swizzle to #RGBA
                var hex = style.Foreground.TrimStart('#');
                var alpha = hex[..2];
                var rgb = hex[2..];
                _builder.Append($"<color=#{rgb}{alpha}>");
            }

            if (style.Italic) _builder.Append("<i>");
            if (style.Bold) _builder.Append("<b>");

            _builder.Append(text);

            if (hasForeground) _builder.Append("</color>");
            if (style.Italic) _builder.Append("</i>");
            if (style.Bold) _builder.Append("</b>");
        }

        private void GetStyleInsertionsForCapturedStyle(Scope scope, ICollection<TextInsertion> styleInsertions) {
            styleInsertions.Add(new TextInsertion {
                Index = scope.Index,
                Scope = scope
            });

            foreach (var childScope in scope.Children)
                GetStyleInsertionsForCapturedStyle(childScope, styleInsertions);

            styleInsertions.Add(new TextInsertion { Index = scope.Index + scope.Length });
        }
    }
}