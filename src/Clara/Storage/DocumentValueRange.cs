using System.Collections;

namespace Clara.Storage
{
    internal readonly struct DocumentValueRange<TValue> : IReadOnlyCollection<int>
        where TValue : struct, IComparable<TValue>
    {
        private readonly IReadOnlyList<DocumentValue<TValue>> source;
        private readonly int lo;
        private readonly int hi;

        public DocumentValueRange(IReadOnlyList<DocumentValue<TValue>> source, TValue? from, TValue? to)
        {
            this.source = source;
            this.lo = 0;
            this.hi = source.Count - 1;

            if (from is not null)
            {
                this.lo = BinarySearchLow(source, from.Value);
            }

            if (to is not null)
            {
                this.hi = BinarySearchHigh(source, to.Value);
            }
        }

        public int Count
        {
            get
            {
                return this.hi - this.lo + 1;
            }
        }

        public Enumerator GetEnumerator()
        {
            return new Enumerator(this);
        }

        IEnumerator<int> IEnumerable<int>.GetEnumerator()
        {
            return new Enumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new Enumerator(this);
        }

        private static int BinarySearchLow(IReadOnlyList<DocumentValue<TValue>> list, TValue value)
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

        private static int BinarySearchHigh(IReadOnlyList<DocumentValue<TValue>> list, TValue value)
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
            private readonly IReadOnlyList<DocumentValue<TValue>> source;
            private readonly int offset;
            private readonly int count;
            private int index;
            private int current;

            public Enumerator(DocumentValueRange<TValue> range)
            {
                this.source = range.source;
                this.offset = range.lo;
                this.count = range.hi - range.lo + 1;
                this.index = 0;
                this.current = default;
            }

            public readonly int Current
            {
                get
                {
                    return this.current;
                }
            }

            readonly object IEnumerator.Current
            {
                get
                {
                    return this.Current;
                }
            }

            public bool MoveNext()
            {
                if (this.index < this.count)
                {
                    this.current = this.source[this.offset + this.index].DocumentId;
                    this.index++;

                    return true;
                }

                this.index = this.count + 1;
                this.current = default;

                return false;
            }

            public void Reset()
            {
                this.index = 0;
                this.current = default;
            }

            public readonly void Dispose()
            {
            }
        }
    }
}
