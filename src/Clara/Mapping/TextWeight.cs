namespace Clara.Mapping
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1815:Override equals and operator equals on value types", Justification = "Not intended to be used directly for comparison")]
    public readonly struct TextWeight
    {
        public const float DefaultWeight = 1;

        public TextWeight(string? text)
            : this(text, DefaultWeight, expandSynonyms: true)
        {
        }

        public TextWeight(string? text, float weight, bool expandSynonyms)
        {
            if (!(weight > 0))
            {
                throw new ArgumentOutOfRangeException(nameof(weight));
            }

            this.Text = text;
            this.Weight = weight;
            this.ExpandSynonyms = expandSynonyms;
        }

        public string? Text { get; }

        public float Weight { get; }

        public bool ExpandSynonyms { get; }
    }
}
