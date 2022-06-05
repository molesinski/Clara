namespace Clara.Utils
{
    internal static class HashHelper
    {
        internal static readonly int[] InitialBuckets = new int[1];

        internal static int PowerOf2(int value)
        {
            if ((value & value - 1) == 0)
            {
                return value;
            }

            var size = 2;

            while (size < value)
            {
                size <<= 1;
            }

            return size;
        }
    }
}
