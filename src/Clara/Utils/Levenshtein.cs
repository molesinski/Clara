// Based on https://github.com/gustf/js-levenshtein

using System.Buffers;

namespace Clara.Utils
{
    public static class Levenshtein
    {
        public static int CalculateDistance(ReadOnlySpan<char> a, ReadOnlySpan<char> b)
        {
            var len = a.Length <= b.Length ? a.Length : b.Length;
            var d = ArrayPool<int>.Shared.Rent(len);

            var dd = CalculateDistance(a, b, d);

            ArrayPool<int>.Shared.Return(d);

            return dd;
        }

        public static int CalculateDistance(ReadOnlySpan<char> a, ReadOnlySpan<char> b, Span<int> d)
        {
            if (a == b)
            {
                return 0;
            }

            if (a.Length > b.Length)
            {
                var tmp = a;
                a = b;
                b = tmp;
            }

            var la = a.Length;
            var lb = b.Length;

            while (la > 0 && (a[la - 1] == b[lb - 1]))
            {
                la--;
                lb--;
            }

            var offset = 0;

            while (offset < la && (a[offset] == b[offset]))
            {
                offset++;
            }

            la -= offset;
            lb -= offset;

            if (la == 0 || lb < 3)
            {
                return lb;
            }

            var x = 0;
            int y;
            int d0;
            int d1;
            int d2;
            int d3;
            var dd = 0;
            int dy;
            char ay;
            char bx0;
            char bx1;
            char bx2;
            char bx3;

            for (y = 0; y < la; y++)
            {
                d[y] = y + 1;
            }

            for (; x < lb - 3;)
            {
                bx0 = b[offset + (d0 = x)];
                bx1 = b[offset + (d1 = x + 1)];
                bx2 = b[offset + (d2 = x + 2)];
                bx3 = b[offset + (d3 = x + 3)];
                dd = x += 4;

                for (y = 0; y < la; y++)
                {
                    dy = d[y];
                    ay = a[offset + y];
                    d0 = Min(dy, d0, d1, bx0, ay);
                    d1 = Min(d0, d1, d2, bx1, ay);
                    d2 = Min(d1, d2, d3, bx2, ay);
                    dd = Min(d2, d3, dd, bx3, ay);
                    d[y] = dd;
                    d3 = d2;
                    d2 = d1;
                    d1 = d0;
                    d0 = dy;
                }
            }

            for (; x < lb;)
            {
                bx0 = b[offset + (d0 = x)];
                dd = ++x;

                for (y = 0; y < la; y++)
                {
                    dy = d[y];
                    d[y] = dd = Min(dy, d0, dd, bx0, a[offset + y]);
                    d0 = dy;
                }
            }

            return dd;

            static int Min(int d0, int d1, int d2, char bx, char ay)
            {
                return (d0 < d1 || d2 < d1)
                    ? d0 > d2
                        ? d2 + 1
                        : d0 + 1
                    : bx == ay
                        ? d1
                        : d1 + 1;
            }
        }
    }
}
