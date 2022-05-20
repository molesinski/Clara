using System;
using System.Collections.Generic;
using System.Linq;
using Clara.Querying;

namespace Clara.Storage
{
    internal sealed class DocumentSort
    {
        private readonly IEnumerable<int> documents;
        private IOrderedEnumerable<int>? orderedDocuments;

        public DocumentSort(IEnumerable<int> documents)
        {
            if (documents is null)
            {
                throw new ArgumentNullException(nameof(documents));
            }

            this.documents = documents;
        }

        public IEnumerable<int> Documents
        {
            get
            {
                return this.orderedDocuments ?? this.documents;
            }
        }

        public void Sort<TValue>(Func<int, TValue> orderer, SortDirection direction)
            where TValue : struct, IComparable<TValue>
        {
            if (this.orderedDocuments == default)
            {
                if (direction == SortDirection.Descending)
                {
                    this.orderedDocuments = this.documents
                        .OrderByDescending(orderer);
                }
                else
                {
                    this.orderedDocuments = this.documents
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
    }
}
