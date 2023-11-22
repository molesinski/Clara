namespace Clara.Storage
{
    internal readonly struct DocumentValue<TValue>
    {
        public DocumentValue(int documentId, TValue value)
        {
            this.DocumentId = documentId;
            this.Value = value;
        }

        public readonly int DocumentId { get; }

        public readonly TValue Value { get; }
    }
}
