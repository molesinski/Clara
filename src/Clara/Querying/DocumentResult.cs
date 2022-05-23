namespace Clara.Querying
{
    public readonly struct DocumentResult<TDocument>
    {
        public DocumentResult(TDocument document, double score)
        {
            this.Document = document;
            this.Score = score;
        }

        public TDocument Document { get; }

        public double Score { get; }
    }
}
