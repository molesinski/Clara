using System.Collections;
using Clara.Utils;

namespace Clara.Querying
{
    public sealed class HierarchyFacetValueCollection : IReadOnlyList<HierarchyFacetValue>, IDisposable
    {
        private readonly ObjectPoolLease<ListSlim<HierarchyFacetValue>> items;
        private readonly int offset;
        private readonly int count;
        private bool isDisposed;

        internal HierarchyFacetValueCollection(
            ObjectPoolLease<ListSlim<HierarchyFacetValue>> items,
            int offset,
            int count)
        {
            this.items = items;
            this.offset = offset;
            this.count = count;
        }

        public int Count
        {
            get
            {
                this.ThrowIfDisposed();

                return this.count;
            }
        }

        public HierarchyFacetValue this[int index]
        {
            get
            {
                this.ThrowIfDisposed();

                if (index < 0 || index >= this.count)
                {
                    throw new ArgumentOutOfRangeException(nameof(index));
                }

                return this.items.Instance[this.offset + index];
            }
        }

        public Enumerator GetEnumerator()
        {
            this.ThrowIfDisposed();

            return new Enumerator(this.items.Instance.GetRangeEnumerator(this.offset, this.count));
        }

        IEnumerator<HierarchyFacetValue> IEnumerable<HierarchyFacetValue>.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public void Dispose()
        {
            if (!this.isDisposed)
            {
                this.items.Dispose();

                this.isDisposed = true;
            }
        }

        private void ThrowIfDisposed()
        {
            if (this.isDisposed)
            {
                throw new ObjectDisposedException(this.GetType().FullName);
            }
        }

        public struct Enumerator : IEnumerator<HierarchyFacetValue>
        {
            private ListSlim<HierarchyFacetValue>.Enumerator enumerator;

            internal Enumerator(ListSlim<HierarchyFacetValue>.Enumerator enumerator)
            {
                this.enumerator = enumerator;
            }

            public readonly HierarchyFacetValue Current
            {
                get
                {
                    return this.enumerator.Current;
                }
            }

            readonly object IEnumerator.Current
            {
                get
                {
                    return this.enumerator.Current;
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
