namespace Clara.Querying
{
    public sealed class SuggestExpression
    {
        public SuggestExpression(PhraseSuggester suggester, string text, int count)
        {
            if (suggester is null)
            {
                throw new ArgumentNullException(nameof(suggester));
            }

            if (text is null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            if (count <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }

            this.Suggester = suggester;
            this.Text = text;
            this.Count = count;
        }

        public PhraseSuggester Suggester { get; }

        public string Text { get; }

        public int Count { get; }
    }
}
