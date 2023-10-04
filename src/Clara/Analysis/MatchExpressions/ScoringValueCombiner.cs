namespace Clara.Analysis.MatchExpressions
{
    internal static class ScoringValueCombiner
    {
        public static Func<float, float, float> Get(ScoringMode scoringMode)
        {
            return scoringMode == ScoringMode.Sum ? Sum : Max;
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
