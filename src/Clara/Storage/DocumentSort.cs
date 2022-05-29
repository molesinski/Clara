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
        private DocumentComparer documentComparer;
        private PooledList<int>? sortedDocuments;

        public DocumentSort(IDocumentSet documentSet)
        {
            if (documentSet is null)
            {
                throw new ArgumentNullException(nameof(documentSet));
            }

            this.documentComparer = DocumentComparer.Empty;
            this.documentSet = documentSet;
        }

        public int Count
        {
            get
            {
                return this.documentSet.Count;
            }
        }

        public void Sort<TValue>(Func<int, TValue> selector, SortDirection direction)
            where TValue : struct, IComparable<TValue>
        {
            this.documentComparer = this.documentComparer.Compose(selector, direction);
        }

        public IEnumerator<int> GetEnumerator()
        {
            if (!this.documentComparer.IsEmpty)
            {
                if (this.sortedDocuments is null)
                {
                    this.sortedDocuments = new PooledList<int>(this.documentSet);
                    this.sortedDocuments.Sort(this.documentComparer);
                }

                return this.sortedDocuments.GetEnumerator();
            }

            return this.documentSet.GetEnumerator();
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
            public static DocumentComparer Empty { get; } = new EmptyDocumentOrderer();

            public abstract bool IsEmpty { get; }

            public abstract int Compare(int x, int y);

            public DocumentComparer Compose<TValue>(Func<int, TValue> selector, SortDirection direction)
                where TValue : struct, IComparable<TValue>
            {
                return new ComposedDocumentComparer<TValue>(this, selector, direction);
            }

            private sealed class EmptyDocumentOrderer : DocumentComparer
            {
                public override bool IsEmpty
                {
                    get
                    {
                        return true;
                    }
                }

                public override int Compare(int x, int y)
                {
                    return 0;
                }
            }

            private sealed class ComposedDocumentComparer<TValue> : DocumentComparer, IComparer<int>
                where TValue : struct, IComparable<TValue>
            {
                private readonly DocumentComparer inner;
                private readonly Func<int, TValue> selector;
                private readonly int direction;

                public ComposedDocumentComparer(DocumentComparer inner, Func<int, TValue> selector, SortDirection direction)
                {
                    if (inner is null)
                    {
                        throw new ArgumentNullException(nameof(inner));
                    }

                    if (selector is null)
                    {
                        throw new ArgumentNullException(nameof(selector));
                    }

                    this.inner = inner;
                    this.selector = selector;
                    this.direction = direction == SortDirection.Descending ? -1 : 1;
                }

                public override bool IsEmpty
                {
                    get
                    {
                        return false;
                    }
                }

                public override int Compare(int x, int y)
                {
                    if (this.inner is not EmptyDocumentOrderer)
                    {
                        var result = this.inner.Compare(x, y);

                        if (result != 0)
                        {
                            return result;
                        }
                    }

                    return this.selector(x).CompareTo(this.selector(y)) * this.direction;
                }
            }
        }
    }
}
