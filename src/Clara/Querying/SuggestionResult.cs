namespace Clara.Querying
{
    public sealed class SuggestionResult
    {
        public SuggestionResult(string text, string highlightedText, int score, float weight, int count)
        {
            if (text is null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            if (highlightedText is null)
            {
                throw new ArgumentNullException(nameof(highlightedText));
            }

            if (count < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }

            this.Text = text;
            this.HighlightedText = highlightedText;
            this.Score = score;
            this.Weight = weight;
            this.Count = count;
        }

        public string Text { get; }

        public string HighlightedText { get; }

        public int Score { get; }

        public float Weight { get; }

        public int Count { get; }
    }
}
