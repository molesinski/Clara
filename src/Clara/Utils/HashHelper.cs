namespace Clara.Utils
{
    internal static class HashHelper
    {
        internal static readonly int[] SizeOneIntArray = new int[1];

        internal static int PowerOf2(int v)
        {
            if ((v & v - 1) == 0)
            {
                return v;
            }

            var i = 2;

            while (i < v)
            {
                i <<= 1;
            }

            return i;
        }
    }
}
