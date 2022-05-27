using System;

namespace Clara.Utils
{
    internal ref struct BitHelper
    {
        private const int IntSize = sizeof(int) * 8;

        private readonly Span<int> span;

        internal BitHelper(Span<int> span, bool clear)
        {
            if (clear)
            {
                span.Clear();
            }

            this.span = span;
        }

        internal void MarkBit(int bitPosition)
        {
            var bitArrayIndex = bitPosition / IntSize;

            if ((uint)bitArrayIndex < (uint)this.span.Length)
            {
                this.span[bitArrayIndex] |= 1 << bitPosition % IntSize;
            }
        }

        internal bool IsMarked(int bitPosition)
        {
            var bitArrayIndex = bitPosition / IntSize;

            return
                (uint)bitArrayIndex < (uint)this.span.Length &&
                (this.span[bitArrayIndex] & 1 << bitPosition % IntSize) != 0;
        }

        internal int FindFirstUnmarked(int startPosition = 0)
        {
            var i = startPosition;

            for (var bi = i / IntSize; (uint)bi < (uint)this.span.Length; bi = ++i / IntSize)
            {
                if ((this.span[bi] & 1 << i % IntSize) == 0)
                {
                    return i;
                }
            }

            return -1;
        }

        internal static int ToIntArrayLength(int n)
        {
            return n > 0 ? (n - 1) / IntSize + 1 : 0;
        }
    }
}
