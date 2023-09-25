using System.Collections;
using Clara.Utils;

namespace Clara.Storage
{
    internal readonly struct DocumentValueRange<TValue> : IReadOnlyCollection<int>
        where TValue : struct, IComparable<TValue>
    {
        private readonly ListSlim<DocumentValue<TValue>>.RangeCollection items;

        public DocumentValueRange(ListSlim<DocumentValue<TValue>> source, TValue? from, TValue? to)
        {
            var lo = 0;
            var hi = source.Count - 1;

            if (from is not null)
            {
                lo = BinarySearchLow(source, from.Value);
            }

            if (to is not null)
            {
                hi = BinarySearchHigh(source, to.Value);
            }

            this.items = source.Range(lo, hi - lo + 1);
        }

        public readonly int Count
        {
            get
            {
                return this.items.Count;
            }
        }

        public readonly Enumerator GetEnumerator()
        {
            return new Enumerator(this.items.GetEnumerator());
        }

        readonly IEnumerator<int> IEnumerable<int>.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        readonly IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        private static int BinarySearchLow(ListSlim<DocumentValue<TValue>> list, TValue value)
        {
            var left = 0;
            var right = list.Count - 1;

            while (left <= right)
            {
                var mid = (left + right) / 2;

                if (list[mid].Value.CompareTo(value) < 0)
                {
                    left = mid + 1;
                }
                else
                {
                    right = mid - 1;
                }
            }

            return left;
        }

        private static int BinarySearchHigh(ListSlim<DocumentValue<TValue>> list, TValue value)
        {
            var left = 0;
            var right = list.Count - 1;

            while (left <= right)
            {
                var mid = (left + right) / 2;

                if (list[mid].Value.CompareTo(value) <= 0)
                {
                    left = mid + 1;
                }
                else
                {
                    right = mid - 1;
                }
            }

            return right;
        }

        public struct Enumerator : IEnumerator<int>
        {
            private ListSlim<DocumentValue<TValue>>.Enumerator enumerator;

            public Enumerator(ListSlim<DocumentValue<TValue>>.Enumerator enumerator)
            {
                this.enumerator = enumerator;
            }

            public readonly int Current
            {
                get
                {
                    return this.enumerator.Current.DocumentId;
                }
            }

            readonly object IEnumerator.Current
            {
                get
                {
                    return this.enumerator.Current.DocumentId;
                }
            }

            public bool MoveNext()
            {
                return this.enumerator.MoveNext();
            }

            public void Reset()
            {
                this.enumerator.Reset();
            }

            public void Dispose()
            {
                this.enumerator.Dispose();
            }
        }
    }
}
