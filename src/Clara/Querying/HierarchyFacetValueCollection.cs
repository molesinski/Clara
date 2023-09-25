using System.Collections;
using Clara.Utils;

namespace Clara.Querying
{
    public sealed class HierarchyFacetValueCollection : IReadOnlyCollection<HierarchyFacetValue>, IDisposable
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
                if (this.isDisposed)
                {
                    throw new ObjectDisposedException(this.GetType().FullName);
                }

                return this.count;
            }
        }

        public Enumerator GetEnumerator()
        {
            if (this.isDisposed)
            {
                throw new ObjectDisposedException(this.GetType().FullName);
            }

            return new Enumerator(this.items.Instance.Range(this.offset, this.count).GetEnumerator());
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
