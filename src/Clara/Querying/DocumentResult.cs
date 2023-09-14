namespace Clara.Querying
{
    public readonly record struct DocumentResult<TDocument>
    {
        public DocumentResult(
            string key,
            TDocument document,
            float score)
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
