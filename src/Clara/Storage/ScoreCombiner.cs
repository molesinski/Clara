using Clara.Analysis.MatchExpressions;

namespace Clara.Storage
{
    internal static class ScoreCombiner
    {
        public static Func<float, float, float> Get(ScoreAggregation scoreAggregation)
        {
            return scoreAggregation == ScoreAggregation.Sum ? Sum : Max;
        }

        private static float Sum(float a, float b)
        {
            return a + b;
        }

        private static float Max(float a, float b)
        {
            return a > b ? a : b;
        }
    }
}
