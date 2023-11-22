using Clara.Querying;
using Clara.Utils;

namespace Clara.Storage
{
    internal sealed class ScoreValueCombiner : IValueCombiner<float>
    {
        private float boost;

        public ScoreValueCombiner()
        {
            this.boost = TextSearchField.DefaultBoost;
        }

        public ScoreValueCombiner(float boost)
        {
            this.boost = boost;
        }

        public static ScoreValueCombiner Default { get; } = new ScoreValueCombiner(TextSearchField.DefaultBoost);

        public bool IsDefaultNeutral
        {
            get
            {
                return this.boost == 1;
            }
        }

        public void Initialize(float boost)
        {
            this.boost = boost;
        }

        public float Combine(float a, float b)
        {
            var boosted = b * this.boost;

            return a + boosted;
        }
    }
}
