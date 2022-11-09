using Clara.Querying;

namespace Clara.Storage
{
    internal sealed class DocumentValueComparer<TValue> : IComparer<DocumentValue<TValue>>
        where TValue : struct, IComparable<TValue>
    {
        private readonly int direction;

        private DocumentValueComparer(SortDirection direction)
        {
            this.direction = direction == SortDirection.Descending ? -1 : 1;
        }

        public static IComparer<DocumentValue<TValue>> Ascending { get; } = new DocumentValueComparer<TValue>(SortDirection.Ascending);

        public static IComparer<DocumentValue<TValue>> Descending { get; } = new DocumentValueComparer<TValue>(SortDirection.Descending);

        public int Compare(DocumentValue<TValue> x, DocumentValue<TValue> y)
        {
            return x.Value.CompareTo(y.Value) * this.direction;
        }
    }
}
