using System;
using System.Collections;
using System.Collections.Generic;
using Clara.Querying;
using Clara.Utils;

namespace Clara.Storage
{
    internal sealed class DocumentSort : IDocumentSet
    {
        private readonly IDocumentSet documentSet;
        private DocumentComparer? comparer;
        private PooledList<int>? sortedDocuments;

        public DocumentSort(IDocumentSet documentSet)
        {
            if (documentSet is null)
            {
                throw new ArgumentNullException(nameof(documentSet));
            }

            this.documentSet = documentSet;
        }

        public int Count
        {
            get
            {
                return this.documentSet.Count;
            }
        }

        public void Sort<TValue>(Func<int, TValue> orderer, SortDirection direction)
            where TValue : struct, IComparable<TValue>
        {
            this.comparer = new OrdererDocumentComparer<TValue>(orderer, direction, this.comparer);
        }

        public IEnumerator<int> GetEnumerator()
        {
            if (this.comparer is null)
            {
                return this.documentSet.GetEnumerator();
            }

            if (this.sortedDocuments is null)
            {
                this.sortedDocuments = new PooledList<int>(this.documentSet);
                this.sortedDocuments.Sort(this.comparer);
            }

            return this.sortedDocuments.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public void Dispose()
        {
            this.documentSet.Dispose();
            this.sortedDocuments?.Dispose();
        }

        private abstract class DocumentComparer : IComparer<int>
        {
            protected internal DocumentComparer(DocumentComparer? inner)
            {
                this.Inner = inner;
            }

            protected DocumentComparer? Inner { get; }

            public abstract int Compare(int x, int y);
        }

        private sealed class OrdererDocumentComparer<TValue> : DocumentComparer, IComparer<int>
            where TValue : struct, IComparable<TValue>
        {
            private readonly Func<int, TValue> orderer;
            private readonly int direction;

            public OrdererDocumentComparer(Func<int, TValue> orderer, SortDirection direction, DocumentComparer? inner)
                : base(inner)
            {
                if (orderer is null)
                {
                    throw new ArgumentNullException(nameof(orderer));
                }

                this.orderer = orderer;
                this.direction = direction == SortDirection.Descending ? -1 : 1;
            }

            public override int Compare(int x, int y)
            {
                if (this.Inner is DocumentComparer inner)
                {
                    var result = inner.Compare(x, y);

                    if (result != 0)
                    {
                        return result;
                    }
                }

                return this.orderer(x).CompareTo(this.orderer(y)) * this.direction;
            }
        }
    }
}
