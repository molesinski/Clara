namespace Clara.Mapping
{
    public readonly record struct TextWeight
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
