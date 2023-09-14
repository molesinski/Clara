namespace Clara.Storage
{
    internal interface IDocumentScoring : IDisposable
    {
        bool IsEmpty { get; }

        float GetScore(int documentId);
    }
}
