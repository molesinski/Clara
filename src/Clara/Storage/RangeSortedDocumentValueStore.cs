using System;
using System.Collections.Generic;
using Clara.Mapping;

namespace Clara.Storage
{
    internal class RangeSortedDocumentValueStore<TValue>
        where TValue : struct, IComparable<TValue>
    {
        private readonly List<DocumentValue<TValue>> sortedDocumentValues;

        public RangeSortedDocumentValueStore(
            List<DocumentValue<TValue>> sortedDocumentValues)
        {
            if (sortedDocumentValues is null)
            {
                throw new ArgumentNullException(nameof(sortedDocumentValues));
            }

            sortedDocumentValues.Sort((a, b) => a.Value.CompareTo(b.Value));

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
    }
}
