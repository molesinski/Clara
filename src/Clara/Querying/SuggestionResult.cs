namespace Clara.Querying
{
    public sealed class SuggestionResult
    {
        public SuggestionResult(string text, string highlightedText, int score, float weight)
        {
            if (text is null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            if (highlightedText is null)
            {
                throw new ArgumentNullException(nameof(highlightedText));
            }

            this.Text = text;
            this.HighlightedText = highlightedText;
            this.Score = score;
            this.Weight = weight;
        }

        public string Text { get; }

        public string HighlightedText { get; }

        public int Score { get; }

        public float Weight { get; }
    }
}
