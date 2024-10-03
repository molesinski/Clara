using Clara.Utils;

namespace Clara.Storage
{
    public static class ValueCombiner
    {
        public static IValueCombiner<float> Sum { get; } = new SumValueCombiner();

        public static IValueCombiner<float> Max { get; } = new MaxValueCombiner();

        private sealed class SumValueCombiner : IValueCombiner<float>
        {
            public bool IsDefaultNeutral
            {
                get
                {
                    return true;
                }
            }

            public float Combine(float a, float b)
            {
                return a + b;
            }
        }

        private sealed class MaxValueCombiner : IValueCombiner<float>
        {
            public bool IsDefaultNeutral
            {
                get
                {
                    return true;
                }
            }

            public float Combine(float a, float b)
            {
                return a > b ? a : b;
            }
        }
    }
}
