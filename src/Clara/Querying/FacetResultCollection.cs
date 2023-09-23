using System.Collections;
using Clara.Utils;

namespace Clara.Querying
{
    public sealed class FacetResultCollection : IReadOnlyCollection<FacetResult>, IDisposable
    {
        private readonly ObjectPoolLease<ListSlim<FacetResult>> facetResults;
        private bool isDisposed;

        internal FacetResultCollection(
            ObjectPoolLease<ListSlim<FacetResult>> facetResults)
        {
            this.facetResults = facetResults;
        }

        public int Count
        {
            get
            {
                if (this.isDisposed)
                {
                    throw new ObjectDisposedException(this.GetType().FullName);
                }

                return this.facetResults.Instance.Count;
            }
        }

        public Enumerator GetEnumerator()
        {
            if (this.isDisposed)
            {
                throw new ObjectDisposedException(this.GetType().FullName);
            }

            return new Enumerator(this.facetResults.Instance);
        }

        IEnumerator<FacetResult> IEnumerable<FacetResult>.GetEnumerator()
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
                foreach (var facet in this.facetResults.Instance)
                {
                    facet.Dispose();
                }

                this.facetResults.Dispose();

                this.isDisposed = true;
            }
        }

        public struct Enumerator : IEnumerator<FacetResult>
        {
            private readonly ListSlim<FacetResult> facetResults;
            private readonly int count;
            private int index;
            private FacetResult current;

            internal Enumerator(ListSlim<FacetResult> facetResults)
            {
                this.facetResults = facetResults;
                this.count = facetResults.Count;
                this.index = 0;
                this.current = default!;
            }

            public readonly FacetResult Current
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
                    return this.current;
                }
            }

            public bool MoveNext()
            {
                if (this.index < this.count)
                {
                    this.current = this.facetResults[this.index];
                    this.index++;

                    return true;
                }

                this.index = this.count + 1;
                this.current = default!;

                return false;
            }

            public void Reset()
            {
                this.index = 0;
                this.current = default!;
            }

            public readonly void Dispose()
            {
            }
        }
    }
}
