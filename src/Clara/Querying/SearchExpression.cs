using Clara.Mapping;

namespace Clara.Querying
{
    public sealed class SearchExpression
    {
        public SearchExpression(TextField field, string text, SearchMode mode)
        {
            if (field is null)
            {
                throw new ArgumentNullException(nameof(field));
            }

            if (text is null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            if (mode != SearchMode.All && mode != SearchMode.Any)
            {
                throw new ArgumentOutOfRangeException(nameof(mode));
            }

            this.Field = field;
            this.Text = text;
            this.Mode = mode;
        }

        public TextField Field { get; }

        public string Text { get; }

        public SearchMode Mode { get; }

        internal bool IsEmpty
        {
            get
            {
                return string.IsNullOrWhiteSpace(this.Text);
            }
        }
    }
}
