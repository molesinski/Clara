using Clara.Querying;
using Clara.Utils;

namespace Clara.Storage
{
    internal sealed class ScoreCombiner : IValueCombiner<float>
    {
        private float boost;

        public ScoreCombiner()
        {
            this.boost = SearchField.DefaultBoost;
        }

        public ScoreCombiner(float boost)
        {
            this.boost = boost;
        }

        public static ScoreCombiner Default { get; } = new ScoreCombiner(SearchField.DefaultBoost);

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
            return a + (b * this.boost);
        }
    }
}
