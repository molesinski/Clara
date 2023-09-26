using System.Collections;
using Clara.Utils;

namespace Clara.Querying
{
    public class HierarchyFacetValueChildrenCollection : IReadOnlyCollection<HierarchyFacetValue>
    {
        private readonly ListSlim<HierarchyFacetValue> items;
        private readonly int offset;
        private readonly int count;

        internal HierarchyFacetValueChildrenCollection(
            ListSlim<HierarchyFacetValue> items,
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
                return this.count;
            }
        }

        public Enumerator GetEnumerator()
        {
            return new Enumerator(this.items.GetRangeEnumerator(this.offset, this.count));
        }

        IEnumerator<HierarchyFacetValue> IEnumerable<HierarchyFacetValue>.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
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
