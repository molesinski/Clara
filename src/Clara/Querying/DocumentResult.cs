namespace Clara.Querying
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1815:Override equals and operator equals on value types", Justification = "Not intended to be used directly for comparison")]
    public readonly struct DocumentResult<TDocument>
    {
        public DocumentResult(string key, TDocument document, float score)
        {
            this.Key = key;
            this.Document = document;
            this.Score = score;
        }

        public string Key { get; }

        public TDocument Document { get; }

        public float Score { get; }
    }
}
