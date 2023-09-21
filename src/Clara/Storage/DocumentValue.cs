namespace Clara.Storage
{
    internal readonly struct DocumentValue<TValue> : IComparable<DocumentValue<TValue>>
        where TValue : struct, IComparable<TValue>
    {
        public DocumentValue(int documentId, TValue value)
        {
            this.DocumentId = documentId;
            this.Value = value;
        }

        public readonly int DocumentId { get; }

        public readonly TValue Value { get; }

        public readonly int CompareTo(DocumentValue<TValue> other)
        {
            return this.Value.CompareTo(other.Value);
        }
    }
}
