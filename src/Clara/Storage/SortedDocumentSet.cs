using System.Collections;

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
}
