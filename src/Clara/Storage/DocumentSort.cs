using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Clara.Querying;

namespace Clara.Storage
{
    internal sealed class DocumentSort : IDocumentSet
    {
        private readonly IDocumentSet documentSet;
        private IOrderedEnumerable<int>? orderedDocuments;

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
            if (this.orderedDocuments == default)
            {
                if (direction == SortDirection.Descending)
                {
                    this.orderedDocuments = this.documentSet
                        .OrderByDescending(orderer);
                }
                else
                {
                    this.orderedDocuments = this.documentSet
                        .OrderBy(orderer);
                }
            }
            else
            {
                if (direction == SortDirection.Descending)
                {
                    this.orderedDocuments = this.orderedDocuments
                        .ThenByDescending(orderer);
                }
                else
                {
                    this.orderedDocuments = this.orderedDocuments
                        .ThenBy(orderer);
                }
            }
        }

        public void Dispose()
        {
            this.documentSet.Dispose();
        }

        public IEnumerator<int> GetEnumerator()
        {
            return this.orderedDocuments?.GetEnumerator() ?? this.documentSet.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.orderedDocuments?.GetEnumerator() ?? this.documentSet.GetEnumerator();
        }
    }
}
