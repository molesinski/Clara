using Clara.Analysis.MatchExpressions;
using Clara.Querying;
using Clara.Utils;

namespace Clara.Storage
{
    internal sealed class ScoreCombiner : IValueCombiner<float>
    {
        private ScoreAggregation scoreAggregation;
        private float boost;

        public ScoreCombiner()
        {
            this.scoreAggregation = ScoreAggregation.Sum;
            this.boost = SearchField.DefaultBoost;
        }

        public ScoreCombiner(ScoreAggregation scoreAggregation, float boost)
        {
            this.scoreAggregation = scoreAggregation;
            this.boost = boost;
        }

        public static ScoreCombiner Sum { get; } = new ScoreCombiner(ScoreAggregation.Sum, SearchField.DefaultBoost);

        public static ScoreCombiner Max { get; } = new ScoreCombiner(ScoreAggregation.Max, SearchField.DefaultBoost);

        public bool IsDefaultNeutral
        {
            get
            {
                return this.boost == 1;
            }
        }

        public static ScoreCombiner For(ScoreAggregation scoreAggregation)
        {
            return scoreAggregation == ScoreAggregation.Sum ? Sum : Max;
        }

        public void Initialize(ScoreAggregation scoreAggregation, float boost)
        {
            this.scoreAggregation = scoreAggregation;
            this.boost = boost;
        }

        public float Combine(float a, float b)
        {
            var boosted = b * this.boost;

            if (this.scoreAggregation == ScoreAggregation.Sum)
            {
                return a + boosted;
            }
            else
            {
                return a > boosted ? a : boosted;
            }
        }
    }
}
