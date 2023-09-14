namespace Clara.Storage
{
    internal static class ValueCombiner
    {
        public static float Sum(float a, float b)
        {
            return a + b;
        }

        public static float Max(float a, float b)
        {
            return a > b ? a : b;
        }
    }
}
