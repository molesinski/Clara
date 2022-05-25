using System;
using System.Collections.Generic;
using System.Linq;
using Clara.Querying;

namespace Clara.Storage
{
    internal sealed class DocumentSort : IDisposable
    {
        private readonly DocumentSet documentSet;
        private IOrderedEnumerable<int>? orderedDocuments;

        public DocumentSort(DocumentSet documentSet)
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
                return this.documentSet.Documents.Count;
            }
        }

        public IEnumerable<int> Documents
        {
            get
            {
                return this.orderedDocuments ?? (IEnumerable<int>)this.documentSet.Documents;
            }
        }

        public void Sort<TValue>(Func<int, TValue> orderer, SortDirection direction)
            where TValue : struct, IComparable<TValue>
        {
            if (this.orderedDocuments == default)
            {
                if (direction == SortDirection.Descending)
                {
                    this.orderedDocuments = this.documentSet.Documents
                        .OrderByDescending(orderer);
                }
                else
                {
                    this.orderedDocuments = this.documentSet.Documents
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
    }
}
