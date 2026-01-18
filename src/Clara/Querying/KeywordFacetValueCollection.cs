using System.Collections;
using Clara.Utils;

namespace Clara.Querying
{
    public sealed class KeywordFacetValueCollection : IReadOnlyList<KeywordFacetValue>, IDisposable
    {
        private readonly ObjectPoolSlimLease<ListSlim<KeywordFacetValue>> items;
        private bool isDisposed;

        internal KeywordFacetValueCollection(
            ObjectPoolSlimLease<ListSlim<KeywordFacetValue>> items)
        {
            this.items = items;
        }

        public int Count
        {
            get
            {
                this.ThrowIfDisposed();

                return this.items.Instance.Count;
            }
        }

        public KeywordFacetValue this[int index]
        {
            get
            {
                this.ThrowIfDisposed();

                return this.items.Instance[index];
            }
        }

        public Enumerator GetEnumerator()
        {
            this.ThrowIfDisposed();

            return new Enumerator(this.items.Instance.GetEnumerator());
        }

        IEnumerator<KeywordFacetValue> IEnumerable<KeywordFacetValue>.GetEnumerator()
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

        public struct Enumerator : IEnumerator<KeywordFacetValue>
        {
            private ListSlim<KeywordFacetValue>.Enumerator enumerator;

            internal Enumerator(ListSlim<KeywordFacetValue>.Enumerator enumerator)
            {
                this.enumerator = enumerator;
            }

            public readonly KeywordFacetValue Current
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
