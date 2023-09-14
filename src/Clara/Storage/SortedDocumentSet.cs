using System.Collections;
using Clara.Utils;

namespace Clara.Storage
{
    internal abstract class SortedDocumentSet : IDocumentSet
    {
        protected internal SortedDocumentSet()
        {
        }

        public abstract int Count { get; }

        public abstract IEnumerator<int> GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public abstract void Dispose();
    }

    internal sealed class SortedDocumentSet<TValue> : SortedDocumentSet
        where TValue : struct, IComparable<TValue>
    {
        private readonly PooledList<DocumentValue<TValue>> sortedDocuments;

        public SortedDocumentSet(
            IDocumentSet documentSet,
            Func<int, TValue> valueSelector,
            IComparer<DocumentValue<TValue>> comparer)
        {
            if (documentSet is null)
            {
                throw new ArgumentNullException(nameof(documentSet));
            }

            if (valueSelector is null)
            {
                throw new ArgumentNullException(nameof(valueSelector));
            }

            if (comparer is null)
            {
                throw new ArgumentNullException(nameof(comparer));
            }

            this.sortedDocuments = new PooledList<DocumentValue<TValue>>(Allocator.ArrayPool, capacity: documentSet.Count);

            foreach (var documentId in documentSet)
            {
                var value = valueSelector(documentId);

                this.sortedDocuments.Add(new DocumentValue<TValue>(documentId, value));
            }

            this.sortedDocuments.Sort(comparer);

            documentSet.Dispose();
        }

        public override int Count
        {
            get
            {
                return this.sortedDocuments.Count;
            }
        }

        public override IEnumerator<int> GetEnumerator()
        {
            return this.sortedDocuments
                .Select(o => o.DocumentId)
                .GetEnumerator();
        }

        public override void Dispose()
        {
            this.sortedDocuments.Dispose();
        }
    }
}
