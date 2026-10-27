namespace Clara.Mapping
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1815:Override equals and operator equals on value types", Justification = "Not intended to be used directly for comparison")]
    public readonly struct PhraseWeight
    {
        public PhraseWeight(string? phrase, float weight)
        {
            if (!(weight >= 0) || float.IsInfinity(weight))
            {
                throw new ArgumentOutOfRangeException(nameof(weight));
            }

            this.Phrase = phrase;
            this.Weight = weight;
        }

        public string? Phrase { get; }

        public float Weight { get; }
    }
}
