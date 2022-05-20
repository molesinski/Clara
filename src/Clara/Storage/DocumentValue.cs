using System;

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

        public int DocumentId { get; }

        public TValue Value { get; }

        public int CompareTo(DocumentValue<TValue> other)
        {
            return this.Value.CompareTo(other.Value);
        }
    }
}
