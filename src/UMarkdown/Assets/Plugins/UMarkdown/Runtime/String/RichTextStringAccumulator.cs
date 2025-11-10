using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Markdig.Helpers;
using UMarkdown.Parsing;

namespace UMarkdown.String {
    public class RichTextStringAccumulator : IRichTextConsumer {
        private readonly StringBuilder _builder = new();
        private int _indentLevel;
        private bool _isNewline = true;
        private bool _silentIndent;

        public void AcceptCodeBlock(RichTextRenderer renderer, string richText) {
            AppendLine(richText);
        }

        public void AcceptTextBlock(RichTextRenderer renderer, string richText) {
            var textStyle = renderer.TextStyleProvider.GetDefaultTextStyle(renderer);
            var inner = new StringBuilder();
            textStyle.Begin(inner);
            inner.Append(richText.Trim());
            textStyle.End(inner);
            AppendLine(inner.ToString());
        }

        public void BeginIndent(RichTextRenderer renderer, string prefix = null) {
            if (_silentIndent)
                _silentIndent = false;
            else if (!EndsWith("\n<br>")) EnsureLine(true);

            _indentLevel++;
            if (prefix != null) Append(prefix);
            else _silentIndent = true;
            AppendMeta($"<indent={_indentLevel * renderer.Style.IntentSize}px>");
        }

        public void EndIndent(RichTextRenderer renderer) {
            _indentLevel--;
            TrimRight();
            AppendMeta("</indent>");
        }

        public void HintNewline(RichTextRenderer renderer) {
            EnsureLine(true);
        }

        public void EnsureVerticalSpace(RichTextRenderer renderer) {
            AppendMeta("<br>");
        }

        public object Finalize(RichTextRenderer renderer) {
            return _builder.ToString();
        }

        private void EnsureLine(bool force = false) {
            if (_builder.Length == 0) return;
            if (_isNewline && !force) return;
            if (_builder[^1] == '\n') return;
            _builder.AppendLine();
            _isNewline = true;
        }

        private void TrimRight() {
            while (_builder.Length > 1 && _builder[^1].IsWhitespace()) _builder.Length--;
        }

        private void Append(string text) {
            _builder.Append(text);
            _isNewline = false;
        }

        private void AppendLine(string text) {
            _builder.AppendLine(text.TrimEnd());
            _isNewline = true;
        }

        private void AppendMeta(string text) {
            _builder.Append(text);
        }

        public override string ToString() {
            return _builder.ToString();
        }

        private bool EndsWith(string end) {
            if (end.Length > _builder.Length) return false;

            var startIndex = _builder.Length - end.Length;
            return !end.Where((t, i) => _builder[startIndex + i] != t).Any();
        }
    }
}