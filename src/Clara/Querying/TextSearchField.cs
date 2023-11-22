using Clara.Mapping;

namespace Clara.Querying
{
    public readonly record struct TextSearchField
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
