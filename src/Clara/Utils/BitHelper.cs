namespace Clara.Utils
{
    internal ref struct BitHelper
    {
        private const int IntSize = sizeof(int) * 8;

        private readonly Span<int> span;

        public BitHelper(Span<int> span, bool clear)
        {
            if (clear)
            {
                span.Clear();
            }

            this.span = span;
        }

        internal static int ToIntArrayLength(int n)
        {
            return n > 0 ? (((n - 1) / IntSize) + 1) : 0;
        }

        internal void MarkBit(int bitPosition)
        {
            var bitArrayIndex = (uint)bitPosition / IntSize;
            var span = this.span;

            if (bitArrayIndex < (uint)span.Length)
            {
                span[(int)bitArrayIndex] |= 1 << (int)((uint)bitPosition % IntSize);
            }
        }

        internal readonly bool IsMarked(int bitPosition)
        {
            var bitArrayIndex = (uint)bitPosition / IntSize;
            var span = this.span;

            return bitArrayIndex < (uint)span.Length
                && (span[(int)bitArrayIndex] & (1 << ((int)((uint)bitPosition % IntSize)))) != 0;
        }
    }
}
