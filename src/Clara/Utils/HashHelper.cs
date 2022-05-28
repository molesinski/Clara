using System;

namespace Clara.Utils
{
    internal static class HashHelper
    {
        private const int MinimumCapacity = 16;

        internal static readonly int[] InitialBuckets = new int[1];

        internal static int Size(int capacity)
        {
            capacity = Math.Max(capacity, MinimumCapacity);

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
