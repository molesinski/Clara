using Clara.Utils;

namespace Clara.Mapping
{
    public abstract class ScoreAggregation : IValueCombiner<float>
    {
        public static ScoreAggregation Default
        {
            get
            {
                return Sum;
            }
        }

        public static ScoreAggregation Sum { get; } = new SumScoreAggregation();

        public static ScoreAggregation Max { get; } = new MaxScoreAggregation();

        public virtual float Combine(float current, float value)
        {
            return this.Combine(current, value, 1f);
        }

        public abstract float Combine(float current, float value, float valueBoost);

        private sealed class SumScoreAggregation : ScoreAggregation
        {
            public override float Combine(float current, float value, float valueBoost)
            {
                var boosted = value * valueBoost;

                return current + boosted;
            }
        }

        private sealed class MaxScoreAggregation : ScoreAggregation
        {
            public override float Combine(float current, float value, float valueBoost)
            {
                var boosted = value * valueBoost;

                return current > boosted ? current : boosted;
            }
        }
    }
}
