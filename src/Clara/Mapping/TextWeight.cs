namespace Clara.Mapping
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1815:Override equals and operator equals on value types", Justification = "Not intended to be used directly for comparison")]
    public readonly struct TextWeight
    {
        public const float DefaultWeight = 1;

        public TextWeight(string? text)
            : this(text, DefaultWeight)
        {
        }

        public TextWeight(string? text, float weight)
        {
            if (!(weight > 0))
            {
                throw new ArgumentOutOfRangeException(nameof(weight));
            }

            this.Text = text;
            this.Weight = weight;
        }

        public string? Text { get; }

        public float Weight { get; }
    }
}
