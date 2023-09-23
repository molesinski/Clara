namespace Clara.Utils
{
    internal static class ArraySortHelper<TItem>
    {
        private const int SizeThreshold = 16;

        public static void Sort(TItem[] source, int index, int count, IComparer<TItem> comparer)
        {
            if (count < 2)
            {
                return;
            }

            IntrospectiveSort(source, index, count + index - 1, 2 * FloorLog2PlusOne(count), comparer);
        }

        private static void IntrospectiveSort(TItem[] source, int lo, int hi, int depthLimit, IComparer<TItem> comparer)
        {
            while (hi > lo)
            {
                var partitionSize = hi - lo + 1;

                if (partitionSize <= SizeThreshold)
                {
                    if (partitionSize == 1)
                    {
                        return;
                    }

                    if (partitionSize == 2)
                    {
                        SwapIfGreater(source, comparer, lo, hi);

                        return;
                    }

                    if (partitionSize == 3)
                    {
                        SwapIfGreater(source, comparer, lo, hi - 1);
                        SwapIfGreater(source, comparer, lo, hi);
                        SwapIfGreater(source, comparer, hi - 1, hi);

                        return;
                    }

                    InsertionSort(source, lo, hi, comparer);

                    return;
                }

                if (depthLimit == 0)
                {
                    Heapsort(source, lo, hi, comparer);

                    return;
                }

                depthLimit--;

                var p = PickPivotAndPartition(source, lo, hi, comparer);

                IntrospectiveSort(source, p + 1, hi, depthLimit, comparer);

                hi = p - 1;
            }
        }

        private static void SwapIfGreater(TItem[] source, IComparer<TItem> comparer, int a, int b)
        {
            if (a != b)
            {
                if (comparer.Compare(source[a], source[b]) > 0)
                {
                    var key = source[a];
                    source[a] = source[b];
                    source[b] = key;
                }
            }
        }

        private static void Swap(TItem[] source, int i, int j)
        {
            if (i != j)
            {
                var t = source[i];
                source[i] = source[j];
                source[j] = t;
            }
        }

        private static int PickPivotAndPartition(TItem[] source, int lo, int hi, IComparer<TItem> comparer)
        {
            var middle = lo + ((hi - lo) / 2);

            SwapIfGreater(source, comparer, lo, middle);
            SwapIfGreater(source, comparer, lo, hi);
            SwapIfGreater(source, comparer, middle, hi);

            var pivot = source[middle];

            Swap(source, middle, hi - 1);

            var left = lo;
            var right = hi - 1;

            while (left < right)
            {
                while (comparer.Compare(source[++left], pivot) < 0)
                {
                }

                while (comparer.Compare(pivot, source[--right]) < 0)
                {
                }

                if (left >= right)
                {
                    break;
                }

                Swap(source, left, right);
            }

            Swap(source, left, hi - 1);

            return left;
        }

        private static void Heapsort(TItem[] source, int lo, int hi, IComparer<TItem> comparer)
        {
            var n = hi - lo + 1;

            for (var i = n / 2; i >= 1; i--)
            {
                DownHeap(source, i, n, lo, comparer);
            }

            for (var i = n; i > 1; i--)
            {
                Swap(source, lo, lo + i - 1);
                DownHeap(source, 1, i - 1, lo, comparer);
            }
        }

        private static void DownHeap(TItem[] source, int i, int n, int lo, IComparer<TItem> comparer)
        {
            var d = source[lo + i - 1];
            int child;

            while (i <= n / 2)
            {
                child = 2 * i;

                if (child < n && comparer.Compare(source[lo + child - 1], source[lo + child]) < 0)
                {
                    child++;
                }

                if (!(comparer.Compare(d, source[lo + child - 1]) < 0))
                {
                    break;
                }

                source[lo + i - 1] = source[lo + child - 1];
                i = child;
            }

            source[lo + i - 1] = d;
        }

        private static void InsertionSort(TItem[] source, int lo, int hi, IComparer<TItem> comparer)
        {
            int i, j;
            TItem t;

            for (i = lo; i < hi; i++)
            {
                j = i;
                t = source[i + 1];

                while (j >= lo && comparer.Compare(t, source[j]) < 0)
                {
                    source[j + 1] = source[j];
                    j--;
                }

                source[j + 1] = t;
            }
        }

        private static int FloorLog2PlusOne(int n)
        {
            var result = 0;

            while (n >= 1)
            {
                result++;
                n /= 2;
            }

            return result;
        }
    }
}
