namespace Clara.Utils
{
    internal static class HashHelper
    {
        internal static readonly int[] InitialBuckets = new int[1];

        internal static int PowerOf2(int capacity)
        {
            if ((capacity & capacity - 1) == 0)
            {
                return capacity;
            }

            var size = 2;

            while (size < capacity)
            {
                size <<= 1;
            }

            return size;
        }
    }
}
