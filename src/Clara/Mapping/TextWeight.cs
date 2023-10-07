namespace Clara.Mapping
{
    public readonly record struct TextWeight
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
