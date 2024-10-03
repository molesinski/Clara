using Clara.Querying;
using Clara.Utils;

namespace Clara.Storage
{
    internal sealed class BoostedValueCombiner : IValueCombiner<float>
    {
        private float boost;

        public BoostedValueCombiner()
        {
            this.boost = TextSearchField.DefaultBoost;
        }

        public float Boost
        {
            get
            {
                return this.boost;
            }

            set
            {
                this.boost = value;
            }
        }

        public bool IsDefaultNeutral
        {
            get
            {
                return this.boost == 1;
            }
        }

        public float Combine(float a, float b)
        {
            return a + (b * this.boost);
        }
    }
}
