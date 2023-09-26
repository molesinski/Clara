﻿using System.Collections;
using Clara.Mapping;
using Clara.Utils;

namespace Clara.Querying
{
    public sealed class FacetResultCollection : IReadOnlyCollection<FacetResult>, IDisposable
    {
        private readonly ObjectPoolLease<ListSlim<FacetResult>> items;
        private bool isDisposed;

        internal FacetResultCollection(
            ObjectPoolLease<ListSlim<FacetResult>> items)
        {
            this.items = items;
        }

        public int Count
        {
            get
            {
                if (this.isDisposed)
                {
                    throw new ObjectDisposedException(this.GetType().FullName);
                }

                return this.items.Instance.Count;
            }
        }

        public KeywordFacetResult Field(KeywordField field)
        {
            if (this.isDisposed)
            {
                throw new ObjectDisposedException(this.GetType().FullName);
            }

            foreach (var facetResult in this.items.Instance)
            {
                if (facetResult.Field == field)
                {
                    return (KeywordFacetResult)facetResult;
                }
            }

            throw new InvalidOperationException("Field facet result not found.");
        }

        public HierarchyFacetResult Field(HierarchyField field)
        {
            if (this.isDisposed)
            {
                throw new ObjectDisposedException(this.GetType().FullName);
            }

            foreach (var facetResult in this.items.Instance)
            {
                if (facetResult.Field == field)
                {
                    return (HierarchyFacetResult)facetResult;
                }
            }

            throw new InvalidOperationException("Field facet result not found.");
        }

        public RangeFacetResult<TValue> Field<TValue>(RangeField<TValue> field)
            where TValue : struct, IComparable<TValue>
        {
            if (this.isDisposed)
            {
                throw new ObjectDisposedException(this.GetType().FullName);
            }

            foreach (var facetResult in this.items.Instance)
            {
                if (facetResult.Field == field)
                {
                    return (RangeFacetResult<TValue>)facetResult;
                }
            }

            throw new InvalidOperationException("Field facet result not found.");
        }

        public Enumerator GetEnumerator()
        {
            if (this.isDisposed)
            {
                throw new ObjectDisposedException(this.GetType().FullName);
            }

            return new Enumerator(this.items.Instance.GetEnumerator());
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
                foreach (var facet in this.items.Instance)
                {
                    facet.Dispose();
                }

                this.items.Dispose();

                this.isDisposed = true;
            }
        }

        public struct Enumerator : IEnumerator<FacetResult>
        {
            private ListSlim<FacetResult>.Enumerator enumerator;

            internal Enumerator(ListSlim<FacetResult>.Enumerator enumerator)
            {
                this.enumerator = enumerator;
            }

            public readonly FacetResult Current
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
