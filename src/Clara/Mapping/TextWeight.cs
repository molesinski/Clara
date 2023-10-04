namespace Clara.Mapping
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1815:Override equals and operator equals on value types", Justification = "Value type used for performance optimization")]
    public readonly struct TextWeight
    {
        public TextWeight(string? text)
        {
            this.Text = text;
            this.Weight = 1;
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
