using Clara.Mapping;

namespace Clara.Querying
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1815:Override equals and operator equals on value types", Justification = "Not intended to be used directly for comparison")]
    public readonly struct TextSearchField
    {
        public const float DefaultBoost = 1;

        public TextSearchField(TextField field)
            : this(field, DefaultBoost)
        {
        }

        public TextSearchField(TextField field, float boost)
        {
            if (field is null)
            {
                throw new ArgumentNullException(nameof(field));
            }

            if (!(boost > 0))
            {
                throw new ArgumentOutOfRangeException(nameof(boost));
            }

            this.Field = field;
            this.Boost = boost;
        }

        public TextField Field { get; }

        public float Boost { get; }
    }
}
