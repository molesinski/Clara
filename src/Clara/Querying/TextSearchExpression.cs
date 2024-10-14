using Clara.Analysis;
using Clara.Mapping;

namespace Clara.Querying
{
    public sealed class TextSearchExpression
    {
        public TextSearchExpression(TextField field, SearchMode searchMode, string text, Func<Position, float>? positionBoost = null)
        {
            if (field is null)
            {
                throw new ArgumentNullException(nameof(field));
            }

            if (searchMode != SearchMode.All && searchMode != SearchMode.Any)
            {
                throw new ArgumentException("Illegal search mode enum value.", nameof(searchMode));
            }

            if (text is null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            this.Field = field;
            this.SearchMode = searchMode;
            this.Text = text;
            this.PositionBoost = positionBoost;
        }

        public TextField Field { get; }

        public SearchMode SearchMode { get; }

        public string Text { get; }

        public Func<Position, float>? PositionBoost { get; }

        internal bool IsEmpty
        {
            get
            {
                return string.IsNullOrWhiteSpace(this.Text);
            }
        }
    }
}
