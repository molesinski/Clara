namespace Clara.Querying
{
    public readonly record struct DocumentResult<TDocument>
    {
        public DocumentResult(TDocument document)
        {
            this.Document = document;
        }

        public TDocument Document { get; }
    }
}
