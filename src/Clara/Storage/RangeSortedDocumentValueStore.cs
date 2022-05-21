using System;
using System.Collections.Generic;
using Clara.Collections;
using Clara.Mapping;

namespace Clara.Storage
{
    internal class RangeSortedDocumentValueStore<TValue> : IDisposable
        where TValue : struct, IComparable<TValue>
    {
        private readonly PooledList<DocumentValue<TValue>> sortedDocumentValues;

        public RangeSortedDocumentValueStore(
            PooledList<DocumentValue<TValue>> sortedDocumentValues)
        {
            if (sortedDocumentValues is null)
            {
                throw new ArgumentNullException(nameof(sortedDocumentValues));
            }

            sortedDocumentValues.Sort(new DocumentValueComparer());

            this.sortedDocumentValues = sortedDocumentValues;
        }

        public double FilterOrder
        {
            get
            {
                return this.sortedDocumentValues.Count;
            }
        }

        public void Filter(Field field, TValue? from, TValue? to, DocumentSet documentSet)
        {
            var rangeMatches = new DocumentValueRange<TValue>(this.sortedDocumentValues, from, to);

            documentSet.IntersectWith(field, rangeMatches);
        }

        public void Dispose()
        {
            this.sortedDocumentValues.Dispose();
        }



        private readonly struct DocumentValueComparer : IComparer<DocumentValue<TValue>>
        {
            public int Compare(DocumentValue<TValue> x, DocumentValue<TValue> y)
            {
                return x.Value.CompareTo(y.Value);
            }
        }
    }
}
